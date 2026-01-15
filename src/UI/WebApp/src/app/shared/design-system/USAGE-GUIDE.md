# 📖 Guía de Uso - Padelea Design System

## Instalación Rápida
```typescript
// En tu componente
import { ButtonComponent, InputComponent, CardComponent } from '@shared/design-system';

@Component({
  imports: [ButtonComponent, InputComponent, CardComponent]
})
```

## 📋 Inventario de Componentes

### Formularios
| Componente | Selector | Propiedades principales |
|------------|----------|------------------------|
| Button | `<ds-button>` | `variant`, `type`, `size`, `loading`, `disabled` |
| Input | `<ds-input>` | `type`, `placeholder`, `error`, `errorMessage` |
| Textarea | `<ds-textarea>` | `rows`, `placeholder`, `error`, `resize` |
| Checkbox | `<ds-checkbox>` | `checked`, `indeterminate`, `disabled` |
| Radio | `<ds-radio>` | `name`, `value`, `checked`, `disabled` |
| Toggle | `<ds-toggle>` | `checked`, `size`, `disabled` |
| Select | `<ds-select>` | `options`, `placeholder`, `multiple` |

### Feedback
| Componente | Selector | Propiedades principales |
|------------|----------|------------------------|
| Toast | `<ds-toast>` | `type`, `message`, `duration`, `dismissible` |
| Alert | `<ds-alert>` | `type`, `title`, `dismissible` |
| Spinner | `<ds-spinner>` | `size`, `color` |
| Progress | `<ds-progress>` | `value`, `max`, `variant`, `showLabel` |
| Skeleton | `<ds-skeleton>` | `variant`, `width`, `height`, `animate` |

### Overlays
| Componente | Selector | Propiedades principales |
|------------|----------|------------------------|
| Modal | `<ds-modal>` | `open`, `size`, `closable`, `title` |
| Drawer | `<ds-drawer>` | `open`, `position`, `size` |
| Tooltip | `<ds-tooltip>` | `content`, `position`, `trigger` |

### Layout
| Componente | Selector | Propiedades principales |
|------------|----------|------------------------|
| Card | `<ds-card>` | `variant`, `padding`, `hoverable` |
| Badge | `<ds-badge>` | `variant`, `color`, `dot` |
| Avatar | `<ds-avatar>` | `src`, `name`, `size`, `status` |
| Tabs | `<ds-tabs>` | `tabs`, `variant`, `activeTab` |
| Accordion | `<ds-accordion>` | `items`, `multiple`, `variant` |

### Navegación
| Componente | Selector | Propiedades principales |
|------------|----------|------------------------|
| Breadcrumb | `<ds-breadcrumb>` | `items`, `separator` |
| Pagination | `<ds-pagination>` | `currentPage`, `totalPages`, `showFirstLast` |
| Stepper | `<ds-stepper>` | `steps`, `currentStep`, `orientation` |

### Datos
| Componente | Selector | Propiedades principales |
|------------|----------|------------------------|
| Table | `<ds-table>` | `columns`, `data`, `sortable`, `selectable` |
| EmptyState | `<ds-empty-state>` | `icon`, `title`, `description` |
| Divider | `<ds-divider>` | `variant`, `label`, `orientation` |

---

## 🚀 Ejemplos de Uso

### Button
```html
<ds-button variant="primary" type="success">Guardar</ds-button>
<ds-button variant="outline" [loading]="isLoading">Enviar</ds-button>
<ds-button variant="ghost" size="sm">Cancelar</ds-button>
```

### Input con validación
```html
<ds-input type="email" placeholder="Email" [error]="hasError" errorMessage="Email inválido" />
```

### Card con contenido
```html
<ds-card variant="elevated" [hoverable]="true">
  <h3 slot="header">Título</h3>
  <p>Contenido de la tarjeta</p>
  <ds-button slot="footer">Acción</ds-button>
</ds-card>
```

### Modal
```html
<ds-modal [open]="showModal" title="Confirmar" (closed)="showModal = false">
  <p>¿Estás seguro de continuar?</p>
</ds-modal>
```

### Table
```html
<ds-table [columns]="columns" [data]="users" [sortable]="true" (rowClick)="onSelect($event)" />
```

### Formulario Reactivo
```typescript
// Los componentes con ControlValueAccessor funcionan con formControlName
<ds-input formControlName="email" placeholder="Email" />
<ds-checkbox formControlName="terms">Acepto los términos</ds-checkbox>
<ds-select formControlName="country" [options]="countries" />
```

### Toast Service
```typescript
// Inyectar servicio y mostrar notificación
this.toastService.show({ type: 'success', message: 'Guardado correctamente' });
```

---

## 🎨 Tokens CSS Disponibles

```css
/* Colores */
--ds-color-brand-primary: #22c55e;
--ds-color-brand-accent: #ff5c35;

/* Tipografía */
--ds-font-family-sans: 'Plus Jakarta Sans';
--ds-font-size-base: 1rem;

/* Espaciado */
--ds-spacing-4: 1rem;
--ds-spacing-8: 2rem;

/* Bordes */
--ds-radius-2xl: 16px;
--ds-radius-3xl: 24px;

/* Sombras */
--ds-shadow-md: 0 4px 6px -1px rgba(0,0,0,0.1);
```

---

## 🔧 Theming

```css
/* Crear tema oscuro */
:root[data-theme="dark"] {
  --color-background: var(--ds-color-gray-900);
  --card-bg: var(--ds-color-gray-800);
  --input-bg: var(--ds-color-gray-700);
}
```

```html
<html data-theme="dark">
```

---

**Versión:** 1.0.0 | **Angular:** 20+ | **Licencia:** Padelea ©
