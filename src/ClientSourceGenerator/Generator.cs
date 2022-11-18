using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.OpenApi.Readers;

namespace ClientSourceGenerator
{
	[Generator]
	public class Generator : ISourceGenerator
	{
		public void Initialize(GeneratorInitializationContext context)
		{
		}

		public void Execute(GeneratorExecutionContext context)
		{
			var oasFiles = GetOasFiles(context);
			var clients = new List<(string, string)>();
			foreach (var (@namespace, file) in oasFiles)
			{
				var controllers = GetControllers(file);
				foreach (var (name, methods) in controllers)
				{
					var code = ClientSource.Generate(@namespace, name, methods);
					context.AddSource($"{@namespace}.{name}Client.generated.cs", SourceText.From(code, Encoding.UTF8));
				}
				clients.AddRange(controllers.Select(c => ($"{@namespace}.I{c.Item1}Client", $"{@namespace}.{c.Item1}Client")));
			}

			var sCode = ServiceSource.Generate(clients);
			context.AddSource("Services.generated.cs", SourceText.From(sCode, Encoding.UTF8));
		}

		static IEnumerable<(string, AdditionalText)> GetOasFiles(GeneratorExecutionContext context)
		{
			var oasFiles = context.AdditionalFiles.Where(f =>
				f.Path.EndsWith(".oas.yaml", StringComparison.OrdinalIgnoreCase) ||
				f.Path.EndsWith(".oas.yml", StringComparison.OrdinalIgnoreCase));
			foreach (var file in oasFiles)
			{
				context.AnalyzerConfigOptions.GetOptions(file).TryGetValue("build_metadata.additionalfiles.Namespace", out var @namespace);
				yield return (@namespace, file);
			}
		}

		static IEnumerable<(string, IList<Method>)> GetControllers(AdditionalText file)
		{
			var stream = new MemoryStream();
			using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
			{
				file.GetText()!.Write(writer);
			}
			stream.Position = 0;
			var doc = new OpenApiStreamReader().Read(stream, out var diagnostic);
			var controllers = new Dictionary<string, IList<Method>>();

			foreach (var p in doc.Paths)
			{
				var path = p.Key;
				var pathItem = p.Value;
				foreach (var op in pathItem.Operations)
				{
					var controller = op.Value.Tags[0].Name;
					if (!controllers.ContainsKey(controller))
					{
						controllers.Add(controller, new List<Method>());
					}
					controllers[controller].Add(new Method { Resource = path, Operation = op });
				}
			}
			return controllers.Select(c => (c.Key, c.Value));
		}
	}
}