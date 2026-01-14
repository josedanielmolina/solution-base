# Core.Application.Tests

Tests unitarios para la capa de aplicación.

## Estructura

```
Core.Application.Tests/
├── Fakers/                    # Generadores de datos con Bogus
│   └── UserFakers.cs
├── Features/                  # Tests de Features (casos de uso)
│   └── Users/
│       ├── CreateUserTests.cs
│       ├── GetUserTests.cs
│       ├── UpdateUserTests.cs
│       └── DeleteUserTests.cs
├── Facades/                   # Tests de Facades
│   └── UserFacadeTests.cs
├── Validators/                # Tests de Validators
│   ├── CreateUserValidatorTests.cs
│   └── UpdateUserValidatorTests.cs
└── Mappings/                  # Tests de Mapping Extensions
    └── UserMappingTests.cs
```

## Convenciones

### Nombrado de Tests

```
[Método]_[Escenario]_[ResultadoEsperado]
```

Ejemplos:
- `ExecuteAsync_WithValidData_ReturnsSuccess`
- `ExecuteAsync_WithExistingEmail_ReturnsEmailAlreadyExistsError`
- `Validate_WithEmptyEmail_ShouldHaveError`

### Patrón AAA

Todos los tests siguen el patrón **Arrange-Act-Assert**:

```csharp
[Fact]
public async Task ExecuteAsync_WithValidData_ReturnsSuccess()
{
    // Arrange - Configurar datos y mocks
    var dto = UserFakers.CreateUserDtoFaker.Generate();
    
    // Act - Ejecutar el código bajo prueba
    var result = await _sut.ExecuteAsync(dto);
    
    // Assert - Verificar resultados
    result.IsSuccess.Should().BeTrue();
}
```

## Ejecución

```bash
# Ejecutar todos los tests de este proyecto
dotnet test tests/Core.Application.Tests

# Ejecutar con cobertura
dotnet test tests/Core.Application.Tests --collect:"XPlat Code Coverage"

# Ejecutar tests específicos
dotnet test tests/Core.Application.Tests --filter "FullyQualifiedName~CreateUser"
```

## Dependencias

- **xUnit**: Framework de testing
- **FluentAssertions**: Aserciones expresivas
- **NSubstitute**: Mocking
- **Bogus**: Generación de datos de prueba
