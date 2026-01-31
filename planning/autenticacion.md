# Fase 1: Autenticación, Perfil y Sistema de Permisos

## 1. Autenticación

* **Método**: Email + contraseña (único método).
* **Recuperación de contraseña**: por correo electrónico.
* **Contraseña temporal**: el admin de plataforma puede asignar una contraseña temporal que requerirá cambio obligatorio en el primer login.
* **Registro de usuarios**: solo el admin de plataforma puede registrar usuarios (de momento).
* **Sesión**: duración parametrizable en appsettings.

## 2. Perfil de Usuario

**Datos del usuario:**
* Nombres (nombres y apellidos) - obligatorio
* Correo electrónico - obligatorio
* Teléfono - opcional
* Documento (DNI, cédula o pasaporte) - obligatorio, sin validación de formato específico

## 3. Vista Principal (Post-Login)

* Listado de eventos donde el usuario es Organizador o Administrador de Evento.
* Acciones disponibles: editar, entrar a evento.
* **Creación de eventos**: Solo el admin de plataforma crea eventos y designa organizador.

## 4. Roles Base

* **Admin de Plataforma**: gestiona categorías globales, registra usuarios, administra a través de un panel separado.
* **Organizador de Evento (Owner)**: creador del evento; control total. Designado por admin de plataforma.
* **Admin de Evento**: usuario con permisos delegados en evento específico. Designado por organizador.
* **Jugador**: participa en eventos.

## 5. Sistema de Permisos

* Un usuario puede tener múltiples roles.
* Los permisos se **suman** (acumulativo) cuando el usuario tiene varios roles.
* El diseño de permisos es **flexible por acciones**, no casado a roles fijos (se asignan acciones a roles según necesidad).
* **Panel de configuración de permisos**: los permisos de cada rol son configurables desde un panel administrativo.

## 6. Matriz de Permisos por Defecto

| Acción | Admin Plataforma | Organizador | Admin Evento |
|--------|------------------|-------------|--------------|
| Crear eventos | ✅ | ❌ | ❌ |
| Designar organizador | ✅ | ❌ | ❌ |
| Editar info del evento | ✅ | ✅ | Configurable |
| Crear/editar/eliminar torneos | ✅ | ✅ | Configurable |
| Gestionar participantes en torneos | ✅ | ✅ | Configurable |
| Configurar fase de grupos | ✅ | ✅ | Configurable |
| Gestionar llave eliminatoria | ✅ | ✅ | Configurable |
| Gestionar establecimientos/canchas | ✅ | ✅ | Configurable |
| Invitar admins de evento | ✅ | ✅ | ❌ |
| Remover admins de evento | ✅ | ✅ | ❌ |
| Gestionar categorías globales | ✅ | ❌ | ❌ |
| Registrar usuarios | ✅ | ❌ | ❌ |

---

**Siguiente:** [eventos.md](./eventos.md)
