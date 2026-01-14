# Infrastructure.Persistence.Tests

Tests de integración para la capa de persistencia con base de datos real.

## Estructura

```
Infrastructure.Persistence.Tests/
├── Fixtures/                  # Fixtures compartidos
│   └── DatabaseFixture.cs     # Contenedor MySQL con Testcontainers
└── Repositories/              # Tests de Repositorios
    └── UserRepositoryTests.cs
```

## Características

### Testcontainers

Este proyecto usa **Testcontainers** para ejecutar MySQL real en Docker durante los tests.

**Requisitos:**
- Docker Desktop instalado y corriendo

### Database Fixture

El `DatabaseFixture` inicia un contenedor MySQL y lo comparte entre tests de la misma colección:

```csharp
[Collection("Database")]
public class UserRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public UserRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetByEmailAsync_WithExistingEmail_ReturnsUser()
    {
        using var context = _fixture.CreateContext();
        // Test con BD real
    }
}
```

## Ejecución

```bash
# Asegúrate de tener Docker corriendo

# Ejecutar todos los tests
dotnet test tests/Infrastructure.Persistence.Tests

# Ejecutar con logs detallados
dotnet test tests/Infrastructure.Persistence.Tests --logger "console;verbosity=detailed"
```

## Notas

- Los tests de integración son más lentos que los unitarios
- Requieren Docker Desktop
- Cada test debe limpiar sus datos o usar datos únicos
