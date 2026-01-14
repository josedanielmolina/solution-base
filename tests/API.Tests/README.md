# API.Tests

Tests de integración para los endpoints de la API.

## Estructura

```
API.Tests/
├── Fixtures/                      # Fixtures compartidos
│   └── CustomWebApplicationFactory.cs
├── Controllers/                   # Tests de Controllers
│   ├── UsersControllerTests.cs
│   └── AuthControllerTests.cs
└── Integration/                   # Tests de flujos completos
    └── AuthenticationFlowTests.cs
```

## CustomWebApplicationFactory

Factory personalizada que configura la API con:
- Base de datos InMemory (no requiere Docker)
- Ambiente de Development
- Servicios mockeados si es necesario

```csharp
public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UsersControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithUsers()
    {
        var response = await _client.GetAsync("/api/users");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
```

## Ejecución

```bash
# Ejecutar todos los tests de API
dotnet test tests/API.Tests

# Ejecutar con filtro
dotnet test tests/API.Tests --filter "FullyQualifiedName~UsersController"
```

## Notas

- Usa InMemory database (no requiere Docker)
- Los tests son más rápidos que con Testcontainers
- Ideal para tests de endpoints y flujos HTTP
