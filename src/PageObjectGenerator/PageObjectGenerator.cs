using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PageObjectSourceGenerator
{
	[Generator]
	public class PageObjectGenerator : ISourceGenerator
	{
		private const string PageObjectAttr = @"using System;

namespace PageObjectSourceGenerator
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    [System.Diagnostics.Conditional(""PageObjectGenerator_DEBUG"")]
    sealed class PageObjectAttribute : Attribute { }
}";

		private const string PageObjectClass = @"using System.Drawing;
using OpenQA.Selenium;

namespace PageObjectSourceGenerator
{
    public class PageObject
    {
        private readonly IWebElement _el;

        public PageObject(IWebElement el) => _el = el;

        public bool Displayed => _el is { Displayed: true };
        public bool Enabled => _el is { Enabled: true };
        public bool Selected => _el is { Selected: true };
        public string Text => _el.Text;
        public string TagName => _el.TagName;
        public Point Location => _el.Location;
        public Size Size => _el.Size;
    }
}";

		public void Initialize(GeneratorInitializationContext context)
		{
			context.RegisterForPostInitialization(i =>
			{
				i.AddSource("PageObjectAttribute", PageObjectAttr);
				i.AddSource("PageObject", PageObjectClass);
			});
			context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
		}

		public void Execute(GeneratorExecutionContext context)
		{
			if (!(context.SyntaxContextReceiver is SyntaxReceiver receiver))
			{
				return;
			}

			var notifySymbol = context.Compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");

			// group the fields by class, and generate the source
			foreach (var group in receiver.Properties.GroupBy(f => f.ContainingType))
			{
				var classSource = GenerateClass(group.Key, group.ToList(), notifySymbol);
				context.AddSource($"{group.Key.Name}.generated.cs", SourceText.From(classSource, Encoding.UTF8));
			}
		}

		private static string GenerateClass(INamedTypeSymbol classSymbol, List<IPropertySymbol> properties, ISymbol notifySymbol)
		{
			if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
			{
				return null; //TODO: issue a diagnostic that it must be top level
			}

			var source = new StringBuilder($@"using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using PageObjectSourceGenerator;

namespace {classSymbol.ContainingNamespace.ToDisplayString()}
{{
    public partial class {classSymbol.Name} : {notifySymbol.ToDisplayString()}
    {{");

			// if the class doesn't implement INotifyPropertyChanged already, add it
			if (!classSymbol.Interfaces.Contains(notifySymbol, SymbolEqualityComparer.Default))
			{
				source.AppendLine(@"
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;");
			}

			foreach (var propertySymbol in properties)
			{
				var propertyName = GetPropertyName(propertySymbol);
				var elementName = propertySymbol.Name;
				var enumerable = propertySymbol.Type.Interfaces.Any(i => i.Name == "IEnumerable");
				if (enumerable)
				{
					source.AppendLine($@"
		public IList<PageObject> {propertyName} => {elementName}.Select(el => new PageObject(el)).ToList();");
				}
				else
				{
					var className = classSymbol.Name;

					source.AppendLine($@"
		public PageObject {propertyName} => new({elementName});");

					if (!MemberExists(classSymbol, $"{classSymbol.ToDisplayString()}.Set{propertyName}(string)"))
					{
						source.AppendLine($@"
		public {className} Set{propertyName}(string text)
		{{
			{elementName}.SendKeys(text);
			return this;
		}}");
					}

					if (!MemberExists(classSymbol, $"{classSymbol.ToDisplayString()}.Clear{propertyName}()"))
					{
						source.AppendLine($@"
		public {className} Clear{propertyName}()
		{{
			{elementName}.Clear();
			return this;
		}}");
					}

					if (!MemberExists(classSymbol, $"{classSymbol.ToDisplayString()}.Click{propertyName}()"))
					{
						source.AppendLine($@"
		public {className} Click{propertyName}()
		{{
			{elementName}.Click();
			return this;
		}}");
					}
				}
			}

			source.Append(@"
	}
}");
			return source.ToString();
		}

		private static string GetPropertyName(IPropertySymbol propertySymbol)
		{
			var elementName = propertySymbol.Name;
			var name = elementName.EndsWith("El") ? elementName.Substring(0, elementName.Length - 2) : elementName;
			string propertyName;
			switch (name.Length)
			{
				case 0:
					propertyName = string.Empty;
					break;
				case 1:
					propertyName = name.ToUpper();
					break;
				default:
					propertyName = name.Substring(0, 1).ToUpper() + name.Substring(1);
					break;
			}

			if (propertyName.Length == 0 || propertyName == elementName)
			{
				//TODO: issue a diagnostic that we can't process this field
			}

			return propertyName;
		}

		private static bool MemberExists(INamedTypeSymbol classSymbol, string definition)
		{
			var member = classSymbol.GetMembers().SingleOrDefault(m => m.OriginalDefinition.ToDisplayString() == definition);
			return member != null;
		}

		private class SyntaxReceiver : ISyntaxContextReceiver
		{
			public IList<IPropertySymbol> Properties { get; } = new List<IPropertySymbol>();

			public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
			{
				// any properties with at least one attribute is a candidate for property generation
				if (context.Node is PropertyDeclarationSyntax pds && pds.AttributeLists.Count > 0)
				{
					// Get the symbol being declared by the property, and keep it if its annotated
					var propSymbol = context.SemanticModel.GetDeclaredSymbol(pds) as IPropertySymbol;
					if (propSymbol.GetAttributes().Any(ad => ad.AttributeClass.ToDisplayString() == "PageObjectSourceGenerator.PageObjectAttribute"))
					{
						Properties.Add(propSymbol);
					}
				}
			}
		}
	}
}