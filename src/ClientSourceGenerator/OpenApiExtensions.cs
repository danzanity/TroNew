using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Linq;

namespace ClientSourceGenerator
{
	public static class OpenApiExtensions
	{
		public static string GetTypeName(this OpenApiSchema schema)
		{
			if (schema.Reference != null) return "object";

			return schema.Type switch
			{
				"array" => "IEnumerable<object>",
				null or "object" => "object",
				"string" => "string",
				"boolean" => "bool",
				"number" => schema.Format ?? "decimal",
				"integer" => schema.Format != null ? $"I{schema.Format.Substring(1)}" : "Int16",
				_ => string.Empty
			};
		}

		public static string GetMethodName(this OpenApiOperation op, string resource, OperationType type)
		{
			var method = op.OperationId;
			if (!string.IsNullOrEmpty(method))
			{
				method = op.OperationId;
				if (op.OperationId.StartsWith($"{op.Tags[0].Name}_"))
				{
					method = method.Substring(op.Tags[0].Name.Length + 1);
				}
			}
			else
			{
				method = resource.Split('/').Where(c => !c.StartsWith("{")).Last();
				if (method == op.Tags[0].Name)
				{
					method = type.ToString();
				}
			}
			return method;
		}

		public static string GetPrimitiveValue(this IOpenApiAny any)
		{
			if (any is OpenApiString)
			{
				return $"\"{(any as OpenApiString)?.Value}\"";
			}
			else if (any is OpenApiBoolean)
			{
				return $"{(any as OpenApiBoolean)?.Value}".ToLower();
			}
			else
			{
				return $"{(any as dynamic)?.Value}";
			}
		}
	}
}
