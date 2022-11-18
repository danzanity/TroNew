# Introduction

Generates TRO api clients using an OpenAPI specification via [source generator](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md#additional-file-transformation).

![diagram](../../.attachments/client-source-generator-diagram.png)

>The [OpenAPI Specification (OAS)](https://github.com/OAI/OpenAPI-Specification) defines a standard, programming language-agnostic interface description for HTTP APIs, which allows both humans and computers to discover and understand the capabilities of a service without requiring access to source code, additional documentation, or inspection of network traffic.

# MSBuild properties
`Demo.csproj`

```xml
<ItemGroup>
  <AdditionalFiles Include="oas.yaml" Namespace="Demo" />
</ItemGroup>

<!-- Manually reference the generator props because we locally reference the generator. When added via NuGet this happens automatically -->
<Import Project="..\ClientSourceGenerator\ClientSourceGenerator.props" />
```

# Sample
`oas.yaml`

```yaml
paths:
  /api/Foo/Bar:
    get:
      tags:
      - Foo
      operationId: Foo_Bar
      responses:
        204:
          description: ''
```

# Output
`Demo.FooClient.generated.cs`

```csharp
public sealed partial class FooClient
{
    private readonly IRestClient _restClient;

    public FooClient(IRestClient restClient)
    {
        _restClient = restClient;
    }

    /// <summary>
    /// <code>/api/Foo/Bar</code>
    /// </summary>
    public async Task Bar(CancellationToken token = default)
    {
        var req = new RestRequest("/api/Foo/Bar", Method.GET);
        var res = await _restClient.ExecuteAsync(req, token);
        res.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
```