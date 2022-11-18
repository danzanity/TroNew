using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.OpenApi.Models;

namespace ClientSourceGenerator
{
	public sealed class ClientSource
	{
		public static string Generate(string @namespace, string controller, IList<Method> methods)
		{
			var interfaceMethods = new List<string>();
			var classMethods = new List<string>();
			foreach (var method in methods)
			{
				var (interfaceMethod, classMethod) = Method(method.Resource, method.Operation);
				interfaceMethods.Add(interfaceMethod);
				classMethods.Add(classMethod);
			}

			return $@"using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Shouldly;

namespace {@namespace}
{{
	public partial interface I{controller}Client
	{{{string.Join(Environment.NewLine, interfaceMethods)}
	}}

	public partial class {controller}Client : I{controller}Client
	{{
		private readonly IRestClient _restClient;

		public {controller}Client(IRestClient restClient)
		{{
			_restClient = restClient;
		}}
{string.Join(Environment.NewLine, classMethods)}
	}}
}}";
		}

		public static (string, string) Method(string resource, KeyValuePair<OperationType, OpenApiOperation> op)
		{
			if (resource.Contains('?'))
			{
				resource = resource.Substring(0, resource.IndexOf('?'));
			}

			var key = op.Key;
			var value = op.Value;
			var paramsDoc = new StringBuilder();
			var args = new List<string>();
			var optArgs = new List<string>();
			var parameters = new StringBuilder();

			var arrayParams = new List<string>();
			foreach (var p in value.Parameters)
			{
				var name = p.Name;
				var type = p.Schema.GetTypeName();
				switch (p.In)
				{
					case ParameterLocation.Query:
						paramsDoc.Append(
							$"{Environment.NewLine}\t\t/// <param name=\"{name}\">{p.Description}</param>");

						if (p.Schema.Default != null)
						{
							optArgs.Add($"{type} {name} = {p.Schema.Default.GetPrimitiveValue()}");
						}
						else
						{
							args.Add($"{type} {name}");
						}

						if (p.Schema.Type == "array")
						{
							arrayParams.Add($@"
			foreach (var _ in {name}) {{ req.AddQueryParameter(""{name}"", $""{{_}}""); }}");
						}
						else
						{
							parameters.Append(
								$"{Environment.NewLine}\t\t\t\t.AddQueryParameter(\"{name}\", $\"{{{name}}}\")");
						}
						break;
					case ParameterLocation.Path:
						paramsDoc.Append(
							$"{Environment.NewLine}\t\t/// <param name=\"{name}\">{p.Description}</param>");
						if (p.Schema.Default != null)
						{
							optArgs.Add($"{type} {name} = {p.Schema.Default.GetPrimitiveValue()}");
						}
						else
						{
							args.Add($"{type} {name}");
						}
						parameters.Append(
							$"{Environment.NewLine}\t\t\t\t.AddParameter(\"{name}\", {name}, ParameterType.UrlSegment)");
						break;
					case ParameterLocation.Header:
						break;
					case ParameterLocation.Cookie:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			if (value.RequestBody != null)
			{
				var name = "payload";
				var type = "";
				if (value.RequestBody.Content.ContainsKey("application/json"))
				{
					var schema = value.RequestBody.Content["application/json"].Schema;
					name = schema.Reference?.Id?.ToCamelCase() ?? name;
					type = schema.GetTypeName();
					parameters.Append($"{Environment.NewLine}\t\t\t\t.AddParameter(\"application/json\", JsonConvert.SerializeObject({name}), ParameterType.RequestBody)");
				}
				else if (value.RequestBody.Content.ContainsKey("multipart/form-data"))
				{
					type = value.RequestBody.Content["multipart/form-data"].Schema.GetTypeName();
				}
				// TODO: ADD MORE REQUEST BODY CONTENT type HERE WHEN NEEDED

				paramsDoc.Append($"{Environment.NewLine}\t\t/// <param name=\"{name}\">{value.RequestBody.Description}</param>");
				args.Add($"{type} {name}");
			}

			args.AddRange(optArgs);
			args.Add("CancellationToken token = default");

			var retStatusCode = "";
			var retDesc = new StringBuilder();
			var retVal = "";
			var ret = "";
			foreach (var r in value.Responses.Where(r => !r.Key.StartsWith("4") && !r.Key.StartsWith("5") && r.Key != "default"))
			{
				retStatusCode = r.Key;
				var response = r.Value;
				if (response.Content.Any())
				{
					if (!string.IsNullOrEmpty(response.Description))
					{
						retDesc.Append($"{Environment.NewLine}\t\t/// <returns>");
						retDesc.Append(string.Join($"<br/>{Environment.NewLine}\t\t/// ", response.Description.SplitByNewline()));
						retDesc.Append("</returns>");
					}

					var type = response.Content.First().Value.Schema.GetTypeName();
					if (new[] { "bool", "Int16", "Int32", "Int64", "decimal", "float", "double", "string" }.Contains(type))
					{
						retVal = $"<{type}>";
						ret = $@"
			return JsonConvert.DeserializeObject<{type}>(res.Content);";
					}
					else if (type == "IEnumerable<object>")
					{
						retVal = "<IList<object>>";
						ret = @"
			return JsonConvert.DeserializeObject<IList<object>>(res.Content);";
					}
					else
					{
						retVal = "<object>";
						ret = @"
			return JsonConvert.DeserializeObject(res.Content);";
					}
				}
			}

			var summary = new StringBuilder();
			summary.Append($"<code>{resource}</code>");

			if (!string.IsNullOrEmpty(value.Summary))
			{
				summary.Append($"<br/>{Environment.NewLine}\t\t/// ");
				summary.Append(string.Join($"<br/>{Environment.NewLine}\t\t/// ", value.Summary.SplitByNewline()));
			}

			if (!string.IsNullOrEmpty(value.Description))
			{
				summary.Append($"<br/>{Environment.NewLine}\t\t/// ");
				summary.Append(string.Join($"<br/>{Environment.NewLine}\t\t/// ", value.Description.SplitByNewline()));
			}

			var method = value.GetMethodName(resource, key);

			var interfaceMethod = $@"
		/// <summary>
		/// {summary}
		/// </summary>{paramsDoc}{retDesc}
		Task{retVal} {method}({string.Join(", ", args)});";

			var classMethod = $@"
		public virtual async Task{retVal} {method}({string.Join(", ", args)})
		{{
			var req = new RestRequest(""{resource}"", Method.{key.ToString().ToUpper()}){parameters};{string.Join("", arrayParams)}
			var res = await _restClient.ExecuteAsync(req, token);
			res.StatusCode.ShouldBe(HttpStatusCode.{(HttpStatusCode)int.Parse(retStatusCode)});{ret}
		}}";
			return (interfaceMethod, classMethod);
		}
	}

	public class Method
	{
		public string Resource { get; set; }
		public KeyValuePair<OperationType, OpenApiOperation> Operation { get; set; }
	}
}