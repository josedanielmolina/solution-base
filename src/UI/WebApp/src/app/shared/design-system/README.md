# 🎨 Padelea Design System

Angular Component Library basado en el Padelea UI Kit. Usa sintaxis moderna de Angular 20+ con Signals.

## 📁 Estructura

```
design-system/
├── tokens/                    # Design tokens primitivos
│   ├── colors.css             # Paleta de colores (green, coral, gray, semantic)
│   ├── typography.css         # Tipografía (Plus Jakarta Sans)
│   ├── spacing.css            # Espaciado (0-96)
│   ├── shadows.css            # Sombras y focus rings
│   ├── borders.css            # Bordes y radios
│   ├── animations.css         # Animaciones y easings
│   └── index.css              # Barrel import
├── themes/                    # Temas (mapeos semánticos)
│   └── default-theme.css      # Tema por defecto
├── components/                # 24 Componentes Angular standalone
│   ├── button/                ├── input/
│   ├── textarea/              ├── checkbox/
│   ├── radio/                 ├── toggle/
│   ├── select/                ├── toast/
│   ├── alert/                 ├── spinner/
│   ├── progress/              ├── skeleton/
│   ├── modal/                 ├── drawer/
│   ├── tooltip/               ├── card/
│   ├── badge/                 ├── avatar/
│   ├── tabs/                  ├── accordion/
│   ├── breadcrumb/            ├── pagination/
│   ├── stepper/               ├── table/
│   ├── empty-state/           └── divider/
├── index.ts                   # Barrel export
└── README.md
```

## 🚀 Instalación

### 1. Importar Estilos

En `angular.json` o `styles.css`:

```json
{
  "styles": [
    "src/app/shared/design-system/tokens/index.css",
    "src/app/shared/design-system/themes/default-theme.css"
  ]
}
```

### 2. Configurar Fuente

```html
<link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700;800&display=swap" rel="stylesheet">
```

### 3. Importar Componentes

```typescript
import { ButtonComponent, InputComponent, CardComponent } from '@shared/design-system';

@Component({
  imports: [ButtonComponent, InputComponent, CardComponent]
})
```

## 🎨 Tokens de Diseño

| Categoría | Primitivo | Semántico |
|-----------|-----------|-----------|
| Color primario | `--ds-color-green-500: #22c55e` | `--color-primary` |
| Color acento | `--ds-color-coral-500: #ff5c35` | `--color-accent` |
| Radio botón | `--ds-radius-2xl: 1rem` | `--btn-border-radius` |
| Radio card | `--ds-radius-3xl: 1.5rem` | `--card-border-radius` |

## 📦 Componentes

### Formularios
- **Button**: 8 variantes, 5 tamaños, estados loading/disabled
- **Input**: Con label, hint, error, password toggle
- **Textarea**: Auto-resize, contador de caracteres
- **Checkbox/Radio/Toggle**: Con descripción
- **Select**: Searchable, clearable, con opciones

### Feedback
- **Toast**: Auto-dismiss, 4 variantes
- **Alert**: Bordered/filled, dismissible
- **Spinner**: 5 tamaños
- **Progress**: Striped, animated, indeterminate
- **Skeleton**: Text, circular, rectangular

### Overlays
- **Modal**: 5 tamaños, slots header/footer
- **Drawer**: 4 posiciones, 5 tamaños
- **Tooltip**: 4 posiciones, dark/light

### Layout
- **Card**: Elevated/outlined/filled, interactive
- **Badge**: 6 colores, outline, dot
- **Avatar**: Imagen/iniciales/fallback, status
- **Tabs**: Underline/pills/boxed
- **Accordion**: Default/bordered/separated

### Navegación
- **Breadcrumb**: 3 separadores
- **Pagination**: First/last, prev/next
- **Stepper**: Horizontal/vertical, clickable

### Datos
- **Table**: Sortable, selectable, striped
- **Empty State**: 5 iconos, acción
- **Divider**: Solid/dashed/dotted, con label

## 🎭 Crear Tema Personalizado

```css
/* themes/dark-theme.css */
:root[data-theme="dark"] {
  --color-background: var(--ds-color-gray-900);
  --color-surface: var(--ds-color-gray-800);
  --card-bg: var(--color-surface);
  --input-bg: var(--ds-color-gray-800);
}
```

```html
<html data-theme="dark">
```

## 🔧 Angular 20+ APIs

Todos los componentes usan:

- **Signals**: `input()`, `output()`, `computed()`, `signal()`
- **Control Flow**: `@if`, `@for`, `@switch`
- **Standalone**: Sin módulos
- **ControlValueAccessor**: Para formularios reactivos

```typescript
@Component({ standalone: true })
export class ButtonComponent {
  readonly variant = input<'primary' | 'secondary'>('primary');
  readonly disabled = input(false);
  readonly click = output<MouseEvent>();
  
  readonly classes = computed(() => `ds-btn--${this.variant()}`);
}
```

## 📄 Licencia

Propiedad de Padelea. Todos los derechos reservados.
<ds-button variant="primary" type="success">Éxito Verde</ds-button>
<ds-button variant="outline">Secundario Outline</ds-button>
<ds-button variant="ghost">Ghost</ds-button>

<!-- Tamaños -->
<ds-button size="xs">Extra Small</ds-button>
<ds-button size="sm">Small</ds-button>
<ds-button size="md">Medium</ds-button>
<ds-button size="lg">Large</ds-button>
<ds-button size="xl">Extra Large</ds-button>

<!-- Estados -->
<ds-button [loading]="true">Cargando...</ds-button>
<ds-button [disabled]="true">Deshabilitado</ds-button>
<ds-button [fullWidth]="true">Ancho Completo</ds-button>

<!-- Con iconos -->
<ds-button>
  <lucide-icon slot="icon-left" name="plus"></lucide-icon>
  Añadir
</ds-button>
```

**CSS Variables del Button:**
```css
--btn-primary-bg: var(--color-brand-accent);
--btn-primary-bg-hover: var(--color-brand-accent-dark);
--btn-primary-text: #ffffff;
--btn-primary-shadow: var(--shadow-accent);
--btn-border-radius: var(--radius-2xl);
```

---

### 2. Input (Campo de texto)

Campo de entrada de texto con validación visual.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `type` | string | `text`, `email`, `password`, `number`, `search`, `tel`, `url` | `text` |
| `placeholder` | string | - | `''` |
| `disabled` | boolean | `true`, `false` | `false` |
| `readonly` | boolean | `true`, `false` | `false` |
| `error` | boolean | `true`, `false` | `false` |
| `errorMessage` | string | - | `''` |

**Uso:**
```html
<ds-input 
  placeholder="Escribe aquí..." 
  label="Input Estándar">
</ds-input>

<ds-input 
  type="email" 
  placeholder="tu@email.com"
  [error]="true"
  errorMessage="Email inválido">
</ds-input>

<ds-input 
  type="password" 
  placeholder="Contraseña"
  [showPasswordToggle]="true">
</ds-input>
```

**CSS Variables del Input:**
```css
--input-bg: var(--color-surface-primary);
--input-border: var(--color-gray-200);
--input-border-focus: var(--color-brand-primary);
--input-border-error: var(--color-danger-500);
--input-border-radius: var(--radius-2xl);
--input-ring-color: rgba(34, 197, 94, 0.1);
```

---

### 3. Card (Tarjeta)

Contenedor premium para agrupar contenido.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `variant` | string | `elevated`, `outlined`, `flat` | `elevated` |
| `hoverable` | boolean | `true`, `false` | `false` |
| `clickable` | boolean | `true`, `false` | `false` |

**Uso:**
```html
<ds-card>
  <ds-card-header>
    <ds-badge variant="success">Status Label</ds-badge>
    <h4>Contenedor Premium</h4>
  </ds-card-header>
  <ds-card-body>
    <p>Contenido de la tarjeta...</p>
  </ds-card-body>
  <ds-card-footer>
    <ds-progress [value]="66"></ds-progress>
  </ds-card-footer>
</ds-card>

<!-- Card interactiva -->
<ds-card [hoverable]="true" [clickable]="true">
  <div class="icon-container">
    <lucide-icon name="dollar-sign"></lucide-icon>
  </div>
  <h4>Feedback al Hover</h4>
  <p>Esta tarjeta reacciona al cursor.</p>
</ds-card>
```

**CSS Variables de Card:**
```css
--card-bg: var(--color-surface-primary);
--card-border: var(--color-border-soft);
--card-border-radius: var(--radius-3xl);
--card-padding: var(--spacing-8);
--card-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.02);
--card-hover-border: var(--color-brand-primary-400);
```

---

### 4. Badge (Insignia)

Etiquetas de estado y categorización.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `variant` | string | `success`, `warning`, `danger`, `info`, `neutral` | `neutral` |
| `size` | string | `xs`, `sm`, `md`, `lg` | `md` |
| `dot` | boolean | `true`, `false` | `false` |

**Uso:**
```html
<ds-badge variant="success">Online</ds-badge>
<ds-badge variant="warning">Inscripción Abierta</ds-badge>
<ds-badge variant="danger">Cerrado</ds-badge>
<ds-badge variant="info">Nuevo</ds-badge>

<!-- Con indicador de punto animado -->
<ds-badge variant="success" [dot]="true" [animated]="true">
  En directo
</ds-badge>
```

**CSS Variables de Badge:**
```css
--badge-success-bg: var(--color-success-100);
--badge-success-text: var(--color-success-600);
--badge-success-border: var(--color-success-200);
--badge-border-radius: var(--radius-full);
```

---

### 5. Modal/Dialog

Diálogos modales para contenido importante.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `size` | string | `sm`, `md`, `lg`, `xl`, `full` | `md` |
| `closable` | boolean | `true`, `false` | `true` |
| `closeOnBackdrop` | boolean | `true`, `false` | `true` |
| `closeOnEscape` | boolean | `true`, `false` | `true` |

**Uso:**
```html
<ds-modal [isOpen]="showModal" (close)="showModal = false">
  <ds-modal-header>
    <h3>Título del Modal</h3>
  </ds-modal-header>
  <ds-modal-body>
    <p>Contenido del modal...</p>
  </ds-modal-body>
  <ds-modal-footer>
    <ds-button variant="outline" (click)="showModal = false">
      Cancelar
    </ds-button>
    <ds-button variant="primary" (click)="confirm()">
      Confirmar
    </ds-button>
  </ds-modal-footer>
</ds-modal>
```

---

### 6. Toast/Notification

Notificaciones flotantes con auto-dismiss.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `type` | string | `success`, `error`, `warning`, `info` | `info` |
| `position` | string | `top-left`, `top-center`, `top-right`, `bottom-left`, `bottom-center`, `bottom-right` | `top-right` |
| `duration` | number | milisegundos | `5000` |
| `dismissible` | boolean | `true`, `false` | `true` |

**Uso con servicio:**
```typescript
// En el componente
constructor(private toastService: ToastService) {}

showSuccess() {
  this.toastService.success('¡Operación exitosa!');
}

showError() {
  this.toastService.error('Ha ocurrido un error');
}

showWithAction() {
  this.toastService.show({
    type: 'warning',
    title: 'Confirmar acción',
    message: '¿Estás seguro?',
    actions: [
      { label: 'Deshacer', handler: () => this.undo() }
    ]
  });
}
```

---

### 7. Spinner/Loader

Indicadores de carga.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `variant` | string | `circular`, `dots`, `linear` | `circular` |
| `size` | string | `sm`, `md`, `lg`, `xl` | `md` |
| `color` | string | CSS color o variable | `--spinner-color` |

**Uso:**
```html
<ds-spinner size="sm"></ds-spinner>
<ds-spinner size="md"></ds-spinner>
<ds-spinner size="lg" color="var(--color-brand-accent)"></ds-spinner>

<!-- Spinner overlay -->
<ds-spinner-overlay [visible]="isLoading">
  Cargando datos...
</ds-spinner-overlay>
```

---

### 8. Progress Bar

Barras de progreso.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `value` | number | 0-100 | `0` |
| `indeterminate` | boolean | `true`, `false` | `false` |
| `showLabel` | boolean | `true`, `false` | `false` |
| `color` | string | `primary`, `success`, `warning`, `danger` | `primary` |

**Uso:**
```html
<ds-progress [value]="66"></ds-progress>
<ds-progress [value]="100" color="success" [showLabel]="true"></ds-progress>
<ds-progress [indeterminate]="true"></ds-progress>
```

---

### 9. Tabs

Navegación por pestañas.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `variant` | string | `line`, `pills`, `enclosed` | `line` |
| `orientation` | string | `horizontal`, `vertical` | `horizontal` |

**Uso:**
```html
<ds-tabs>
  <ds-tab label="General" [icon]="'settings'">
    Contenido de General
  </ds-tab>
  <ds-tab label="Usuarios" [icon]="'users'">
    Contenido de Usuarios
  </ds-tab>
  <ds-tab label="Configuración" [disabled]="true">
    Contenido deshabilitado
  </ds-tab>
</ds-tabs>
```

---

### 10. Table

Tablas de datos con sorting y selección.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `data` | array | - | `[]` |
| `columns` | array | - | `[]` |
| `selectable` | boolean | `true`, `false` | `false` |
| `sortable` | boolean | `true`, `false` | `false` |
| `stickyHeader` | boolean | `true`, `false` | `false` |

**Uso:**
```html
<ds-table 
  [data]="users" 
  [columns]="columns"
  [selectable]="true"
  [sortable]="true"
  (selectionChange)="onSelect($event)"
  (sortChange)="onSort($event)">
</ds-table>
```

```typescript
columns = [
  { key: 'name', label: 'Nombre', sortable: true },
  { key: 'email', label: 'Email', sortable: true },
  { key: 'status', label: 'Estado', template: statusTemplate },
  { key: 'actions', label: 'Acciones', template: actionsTemplate }
];
```

---

### 11. Checkbox

Casillas de verificación.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `checked` | boolean | `true`, `false` | `false` |
| `indeterminate` | boolean | `true`, `false` | `false` |
| `disabled` | boolean | `true`, `false` | `false` |
| `label` | string | - | `''` |

**Uso:**
```html
<ds-checkbox label="Acepto los términos"></ds-checkbox>
<ds-checkbox [(checked)]="isSelected" label="Seleccionar todo" [indeterminate]="isPartial"></ds-checkbox>
<ds-checkbox [disabled]="true" label="Opción deshabilitada"></ds-checkbox>
```

---

### 12. Radio Button

Botones de opción.

**Uso:**
```html
<ds-radio-group [(value)]="selectedOption" name="options">
  <ds-radio value="option1" label="Opción 1"></ds-radio>
  <ds-radio value="option2" label="Opción 2"></ds-radio>
  <ds-radio value="option3" label="Opción 3" [disabled]="true"></ds-radio>
</ds-radio-group>
```

---

### 13. Toggle/Switch

Interruptores de encendido/apagado.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `checked` | boolean | `true`, `false` | `false` |
| `disabled` | boolean | `true`, `false` | `false` |
| `size` | string | `sm`, `md`, `lg` | `md` |
| `label` | string | - | `''` |

**Uso:**
```html
<ds-toggle [(checked)]="isEnabled" label="Activar notificaciones"></ds-toggle>
<ds-toggle size="sm" [(checked)]="darkMode"></ds-toggle>
```

---

### 14. Select/Dropdown

Selectores desplegables.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `options` | array | - | `[]` |
| `multiple` | boolean | `true`, `false` | `false` |
| `searchable` | boolean | `true`, `false` | `false` |
| `placeholder` | string | - | `'Seleccionar...'` |

**Uso:**
```html
<ds-select 
  [options]="countries" 
  placeholder="Selecciona un país"
  [(value)]="selectedCountry">
</ds-select>

<ds-select 
  [options]="tags" 
  [multiple]="true"
  [searchable]="true"
  placeholder="Buscar etiquetas...">
</ds-select>
```

---

### 15. Avatar

Representación visual de usuarios.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `src` | string | URL de imagen | - |
| `initials` | string | - | `''` |
| `size` | string | `xs`, `sm`, `md`, `lg`, `xl`, `2xl` | `md` |
| `status` | string | `online`, `offline`, `busy`, `away` | - |

**Uso:**
```html
<ds-avatar src="user.jpg" size="lg"></ds-avatar>
<ds-avatar initials="JD" size="md"></ds-avatar>
<ds-avatar src="user.jpg" status="online"></ds-avatar>

<!-- Grupo de avatares -->
<ds-avatar-group [max]="3">
  <ds-avatar src="user1.jpg"></ds-avatar>
  <ds-avatar src="user2.jpg"></ds-avatar>
  <ds-avatar src="user3.jpg"></ds-avatar>
  <ds-avatar src="user4.jpg"></ds-avatar>
</ds-avatar-group>
```

---

### 16. Tooltip

Información contextual al hover.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `content` | string | - | `''` |
| `position` | string | `top`, `bottom`, `left`, `right` | `top` |
| `delay` | number | milisegundos | `200` |

**Uso:**
```html
<ds-button dsTooltip="Más información" tooltipPosition="top">
  Hover me
</ds-button>

<!-- O como componente -->
<ds-tooltip content="Información adicional" position="right">
  <span>Texto con tooltip</span>
</ds-tooltip>
```

---

### 17. Breadcrumb

Navegación jerárquica.

**Uso:**
```html
<ds-breadcrumb>
  <ds-breadcrumb-item [link]="'/'">Inicio</ds-breadcrumb-item>
  <ds-breadcrumb-item [link]="'/torneos'">Torneos</ds-breadcrumb-item>
  <ds-breadcrumb-item [active]="true">Torneo de Verano</ds-breadcrumb-item>
</ds-breadcrumb>
```

---

### 18. Pagination

Navegación de páginas.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `total` | number | - | `0` |
| `pageSize` | number | - | `10` |
| `currentPage` | number | - | `1` |
| `showSizeChanger` | boolean | `true`, `false` | `false` |

**Uso:**
```html
<ds-pagination 
  [total]="100" 
  [pageSize]="10"
  [(currentPage)]="page"
  [showSizeChanger]="true"
  (pageChange)="onPageChange($event)">
</ds-pagination>
```

---

### 19. Accordion

Paneles colapsables.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `multiple` | boolean | `true`, `false` | `false` |

**Uso:**
```html
<ds-accordion>
  <ds-accordion-item title="Sección 1" [expanded]="true">
    Contenido de la sección 1
  </ds-accordion-item>
  <ds-accordion-item title="Sección 2">
    Contenido de la sección 2
  </ds-accordion-item>
</ds-accordion>
```

---

### 20. Alert/Banner

Mensajes de alerta.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `type` | string | `success`, `error`, `warning`, `info` | `info` |
| `dismissible` | boolean | `true`, `false` | `false` |
| `icon` | string | nombre del icono | - |

**Uso:**
```html
<ds-alert type="success" [dismissible]="true">
  ¡Registro completado exitosamente!
</ds-alert>

<ds-alert type="warning" icon="alert-triangle">
  Tu suscripción expira en 3 días.
  <ds-alert-action (click)="renovar()">Renovar ahora</ds-alert-action>
</ds-alert>
```

---

### 21. Skeleton

Placeholders de carga.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `variant` | string | `text`, `circular`, `rectangular` | `text` |
| `width` | string | CSS width | `100%` |
| `height` | string | CSS height | `1rem` |

**Uso:**
```html
<!-- Skeleton para texto -->
<ds-skeleton variant="text" width="80%"></ds-skeleton>
<ds-skeleton variant="text" width="60%"></ds-skeleton>

<!-- Skeleton para avatar -->
<ds-skeleton variant="circular" width="3rem" height="3rem"></ds-skeleton>

<!-- Skeleton para card -->
<ds-skeleton variant="rectangular" width="100%" height="200px"></ds-skeleton>
```

---

### 22. Drawer/Sidebar

Paneles laterales deslizantes.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `position` | string | `left`, `right`, `top`, `bottom` | `right` |
| `size` | string | `sm`, `md`, `lg` | `md` |
| `overlay` | boolean | `true`, `false` | `true` |

**Uso:**
```html
<ds-drawer 
  [isOpen]="showDrawer" 
  position="right"
  (close)="showDrawer = false">
  <ds-drawer-header>
    <h3>Filtros</h3>
  </ds-drawer-header>
  <ds-drawer-body>
    <!-- Contenido del drawer -->
  </ds-drawer-body>
</ds-drawer>
```

---

### 23. Popover

Contenido emergente al click.

**Propiedades:**
| Propiedad | Tipo | Valores | Default |
|-----------|------|---------|---------|
| `position` | string | `top`, `bottom`, `left`, `right` | `bottom` |
| `trigger` | string | `click`, `hover`, `focus` | `click` |
| `showArrow` | boolean | `true`, `false` | `true` |

**Uso:**
```html
<ds-popover position="bottom">
  <ds-button slot="trigger">Abrir menú</ds-button>
  <div slot="content">
    <ul>
      <li>Opción 1</li>
      <li>Opción 2</li>
      <li>Opción 3</li>
    </ul>
  </div>
</ds-popover>
```

---

## 🎨 Crear Temas Nuevos

### Paso 1: Crear el archivo del tema

Crea un nuevo archivo CSS en `themes/`:

```css
/* themes/dark-theme.css */

[data-theme="dark"] {
  /* Sobrescribir solo los tokens necesarios */
  
  /* Colores de superficie */
  --color-surface-primary: #1f2937;
  --color-surface-secondary: #111827;
  --color-gray-50: #374151;
  --color-gray-900: #f9fafb;
  
  /* Bordes */
  --color-border-soft: rgba(255, 255, 255, 0.05);
  --color-border-default: #374151;
  
  /* Card */
  --card-bg: #1f2937;
  --card-border: rgba(255, 255, 255, 0.05);
  
  /* Input */
  --input-bg: #374151;
  --input-border: #4b5563;
  --input-text: #f9fafb;
  
  /* Sombras más sutiles para dark mode */
  --shadow-md: 0 4px 6px -1px rgb(0 0 0 / 0.3);
}
```

### Paso 2: Crear un tema de marca personalizado

```css
/* themes/custom-brand-theme.css */

:root {
  /* Cambiar la paleta primaria completa */
  --color-brand-primary: #8b5cf6;  /* Púrpura */
  --color-brand-primary-light: #a78bfa;
  --color-brand-primary-dark: #7c3aed;
  --color-brand-primary-50: #f5f3ff;
  --color-brand-primary-500: #8b5cf6;
  --color-brand-primary-600: #7c3aed;
  --color-brand-primary-700: #6d28d9;
  
  /* Cambiar el acento */
  --color-brand-accent: #ec4899;  /* Rosa */
  --color-brand-accent-dark: #db2777;
  
  /* Cambiar la tipografía */
  --font-sans: 'Poppins', sans-serif;
  
  /* Cambiar los radios */
  --btn-border-radius: var(--radius-full);  /* Botones pill */
  --card-border-radius: var(--radius-2xl);
  --input-border-radius: var(--radius-full);
}
```

### Paso 3: Aplicar el tema

**Opción A: Por atributo data-theme**

```html
<html data-theme="dark">
```

```typescript
// theme.service.ts
@Injectable({ providedIn: 'root' })
export class ThemeService {
  setTheme(theme: 'light' | 'dark' | 'custom') {
    document.documentElement.setAttribute('data-theme', theme);
    localStorage.setItem('theme', theme);
  }
  
  getTheme(): string {
    return localStorage.getItem('theme') || 'light';
  }
}
```

**Opción B: Por clase CSS**

```css
/* themes/dark-theme.css */
.theme-dark {
  --color-surface-primary: #1f2937;
  /* ... resto de tokens */
}
```

```html
<body [class.theme-dark]="isDarkMode">
```

### Paso 4: Importar múltiples temas

```css
/* styles.css */
@import 'app/shared/design-system/themes/default-theme.css';
@import 'app/shared/design-system/themes/dark-theme.css';
@import 'app/shared/design-system/themes/custom-brand-theme.css';
```

---

## 📐 Tokens de Diseño

### Colores

| Token | Descripción | Valor Default |
|-------|-------------|---------------|
| `--color-brand-primary` | Color principal de marca | `#22c55e` (Verde) |
| `--color-brand-accent` | Color de acento/CTA | `#ff5c35` (Coral) |
| `--color-success-*` | Estados de éxito | Verde |
| `--color-warning-*` | Estados de advertencia | Ámbar |
| `--color-danger-*` | Estados de error | Rojo |
| `--color-info-*` | Estados informativos | Azul |
| `--color-gray-*` | Escala de grises | 50-900 |

### Tipografía

| Token | Descripción | Valor Default |
|-------|-------------|---------------|
| `--font-sans` | Fuente principal | Plus Jakarta Sans |
| `--font-mono` | Fuente monoespaciada | JetBrains Mono |
| `--text-xs` a `--text-5xl` | Tamaños de fuente | 0.75rem - 3rem |
| `--font-normal` a `--font-extrabold` | Pesos de fuente | 400 - 800 |

### Espaciado

| Token | Valor |
|-------|-------|
| `--spacing-1` | 0.25rem (4px) |
| `--spacing-2` | 0.5rem (8px) |
| `--spacing-4` | 1rem (16px) |
| `--spacing-8` | 2rem (32px) |
| `--spacing-16` | 4rem (64px) |

### Bordes

| Token | Valor |
|-------|-------|
| `--radius-sm` | 0.25rem (4px) |
| `--radius-md` | 0.375rem (6px) |
| `--radius-lg` | 0.5rem (8px) |
| `--radius-xl` | 0.75rem (12px) |
| `--radius-2xl` | 1rem (16px) |
| `--radius-3xl` | 1.5rem (24px) |
| `--radius-full` | 9999px |

### Sombras

| Token | Uso |
|-------|-----|
| `--shadow-sm` | Elementos sutiles |
| `--shadow-md` | Cards, dropdowns |
| `--shadow-lg` | Popovers, toasts |
| `--shadow-xl` | Modales |
| `--shadow-primary` | Elementos brand |
| `--shadow-accent` | CTAs |

---

## 🔧 Integración con Tailwind CSS

Para usar los tokens con Tailwind, extiende tu configuración:

```typescript
// tailwind.config.ts
import type { Config } from 'tailwindcss';

export default {
  content: ['./src/**/*.{html,ts}'],
  theme: {
    extend: {
      colors: {
        brand: {
          primary: 'var(--color-brand-primary)',
          'primary-light': 'var(--color-brand-primary-light)',
          'primary-dark': 'var(--color-brand-primary-dark)',
          accent: 'var(--color-brand-accent)',
          'accent-dark': 'var(--color-brand-accent-dark)',
        },
        success: {
          50: 'var(--color-success-50)',
          500: 'var(--color-success-500)',
          600: 'var(--color-success-600)',
        },
        // ... resto de colores
      },
      fontFamily: {
        sans: ['var(--font-sans)'],
        mono: ['var(--font-mono)'],
      },
      borderRadius: {
        'card': 'var(--card-border-radius)',
        'btn': 'var(--btn-border-radius)',
        'input': 'var(--input-border-radius)',
      },
    },
  },
} satisfies Config;
```

---

## 🎯 Buenas Prácticas

1. **Usa siempre los tokens**: Nunca uses valores hardcodeados como `#22c55e`. Usa `var(--color-brand-primary)`.

2. **Accesibilidad**: Todos los componentes deben tener soporte para:
   - Navegación por teclado
   - ARIA labels
   - Contraste de colores (WCAG AA mínimo)
   - Focus visible

3. **Componentes standalone**: Cada componente debe ser importable individualmente.

4. **Composición**: Los componentes deben poder combinarse. Por ejemplo:
   ```html
   <ds-input>
     <lucide-icon slot="prefix" name="search"></lucide-icon>
     <ds-button slot="suffix" variant="ghost">
       <lucide-icon name="x"></lucide-icon>
     </ds-button>
   </ds-input>
   ```

5. **Documentación**: Usa Storybook para documentar y probar los componentes visualmente.

---

## 📚 Recursos

- [Lucide Icons](https://lucide.dev/) - Iconos recomendados
- [Plus Jakarta Sans](https://fonts.google.com/specimen/Plus+Jakarta+Sans) - Fuente principal
- [Tailwind CSS](https://tailwindcss.com/) - Framework de utilidades CSS

---

## 📄 Licencia

MIT License - © 2024 Padelea Design System
