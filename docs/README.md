# 📚 Índice de Documentación del Proyecto

Bienvenido a la documentación del proyecto. Este índice te ayudará a encontrar rápidamente la información que necesitas.

---

## 🎯 Skills de Desarrollo

### Arquitectura y Patrones

- **[Backend Architecture (.NET 8)](../.github/skills/backend-architect/SKILL.MD)**
  - Patrones obligatorios (Result Pattern, CQRS, Repository)
  - Estructura del proyecto
  - Validaciones con FluentValidation
  - Clean Architecture

- **[Frontend Architecture (Angular 20)](../.github/skills/frontend-architect/SKILL.md)**
  - Signals y reactividad moderna
  - Standalone components
  - Control Flow Syntax
  - SSR y Hydration

- **[Feature Documentation](../.github/skills/feature-documentation/SKILL.md)** ⭐
  - **Documento vivo** - actualizar con cada cambio
  - Templates y patrones de documentación
  - Proceso obligatorio al finalizar desarrollo
  - Ejemplos y mejores prácticas

---

## 📖 Documentación de Features

### Features Implementados

- **[AUTH - Autenticación](./features/AUTH.md)**
  - Login con email y contraseña
  - JWT tokens
  - Gestión de sesiones

- **[USERS - Gestión de Usuarios](./features/USERS.md)**
  - CRUD completo de usuarios
  - Validaciones y reglas de negocio
  - Hash de contraseñas

---

## 🚀 Quick Start

### Para Desarrolladores Backend

1. Lee el [Backend Architecture Skill](../.github/skills/backend-architect/SKILL.MD)
2. Revisa features existentes en [features/](./features/)
3. **Al terminar tu desarrollo**, actualiza la documentación siguiendo [Feature Documentation Skill](../.github/skills/feature-documentation/SKILL.md)

### Para Desarrolladores Frontend

1. Lee el [Frontend Architecture Skill](../.github/skills/frontend-architect/SKILL.md)
2. Revisa ejemplos de Angular en documentación de features
3. **Al terminar tu desarrollo**, actualiza la sección de Angular en la documentación del feature

---

## 📝 Workflow de Documentación

```
┌─────────────────────┐
│  1. Desarrollar     │
│     Feature/Fix     │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│  2. Código Listo    │
│     (funciona)      │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│  3. ACTUALIZAR      │◄──── ANTES de commit
│     Documentación   │      (obligatorio)
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│  4. Commit con      │
│     Código + Docs   │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│  5. Pull Request    │
│     (revisión)      │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│  6. Merge           │
└─────────────────────┘
```

---

## 🔍 Búsqueda Rápida

### Necesito...

**Crear un nuevo endpoint**
→ [Backend Skill](../.github/skills/backend-architect/SKILL.MD) + [Feature Documentation](../.github/skills/feature-documentation/SKILL.md)

**Consumir API desde Angular**
→ Ver sección "Angular Service Example" en la documentación del feature

**Documentar un feature nuevo**
→ [Feature Documentation Skill](../.github/skills/feature-documentation/SKILL.md) - Sección "Template"

**Actualizar documentación existente**
→ [Feature Documentation Skill](../.github/skills/feature-documentation/SKILL.md) - Sección "Proceso de Actualización"

**Ver ejemplos de buena documentación**
→ [AUTH.md](./features/AUTH.md) o [USERS.md](./features/USERS.md)

---

## ⚠️ Reglas Importantes

### Documentación es Obligatoria

- ✅ La documentación NO es opcional
- ✅ Se actualiza en el mismo PR que el código
- ✅ Se revisa junto con el código
- ✅ Es un documento vivo (se mantiene actualizado)

### Qué Documentar

**Backend:**
- Todos los endpoints (con ejemplos JSON)
- Todos los DTOs
- Validaciones
- Reglas de negocio

**Frontend:**
- Servicios que consumen API
- Guards e interceptors
- Componentes shared importantes
- Interfaces/Types de datos

---

## 📂 Estructura de Carpetas

```
docs/
├── README.md                    # Este archivo (índice principal)
├── features/                    # Documentación de features
│   ├── AUTH.md
│   ├── USERS.md
│   └── [FEATURE-NAME].md
├── api/
│   └── endpoints.md            # Índice rápido de endpoints (futuro)
└── architecture/
    └── patterns.md             # Patrones del proyecto (futuro)

.github/skills/                 # Skills de desarrollo
├── backend-architect/
│   └── SKILL.MD
├── frontend-architect/
│   └── SKILL.md
└── feature-documentation/
    └── SKILL.md               # ⭐ Skill de documentación
```

---

## 🆘 ¿Necesitas Ayuda?

1. **Pregunta 1:** ¿Cómo documento un endpoint nuevo?
   - **Respuesta:** Ver [Feature Documentation Skill](../.github/skills/feature-documentation/SKILL.md) - Sección "Endpoints"

2. **Pregunta 2:** ¿Dónde va mi documentación?
   - **Respuesta:** `docs/features/[FEATURE-NAME].md`

3. **Pregunta 3:** ¿Qué pasa si no documento?
   - **Respuesta:** El PR no será aprobado hasta que incluya documentación actualizada

4. **Pregunta 4:** ¿Cuánto tiempo toma documentar?
   - **Feature nuevo completo:** 30-60 minutos
   - **Actualización de endpoint:** 5-10 minutos
   - **Actualización de modelo:** 5 minutos

---

## 📊 Estado de Documentación

### ✅ Documentado
- [x] AUTH - Authentication feature
- [x] USERS - User management feature

### 📝 Pendiente
- [ ] Tu próximo feature aquí

---

## 🔗 Enlaces Útiles

- [Backend Architecture Skill](../.github/skills/backend-architect/SKILL.MD)
- [Frontend Architecture Skill](../.github/skills/frontend-architect/SKILL.md)
- [Feature Documentation Skill](../.github/skills/feature-documentation/SKILL.md)
- [Semantic Versioning](https://semver.org/)
- [REST API Best Practices](https://swagger.io/resources/articles/best-practices-in-api-documentation/)

---

**Última actualización:** 11 de enero de 2026  
**Mantenido por:** Equipo de Desarrollo
