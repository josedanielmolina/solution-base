---
name: ui-designer
description: Define el sistema de diseño visual con Tailwind CSS para Angular 20. Úsalo cuando necesites estilos, colores, tipografía, componentes UI, dark mode o lineamientos visuales.
---

# 🎾 PadelTourney UI Design System

> **Skill de lineamientos visuales para Angular 20 + Tailwind CSS**
> Aplicación de gestión de torneos de pádel

**📎 Skill Relacionado:** Para arquitectura, patrones y convenciones de desarrollo, ver [Frontend Architecture Skill](./skill-frontend.md)

**⚠️ IMPORTANTE - ARCHIVOS SEPARADOS:**
- Los ejemplos de componentes Angular en este documento son **solo ilustrativos**
- En código de producción, **SIEMPRE** usar archivos separados: `.ts`, `.html`, `.css`
- Ver [Frontend Architecture Skill](./skill-frontend.md) para reglas de estructura de archivos

---

## 📋 Índice

1. [Filosofía de Diseño](#filosofía-de-diseño)
2. [Mobile First - Diseño Responsivo](#mobile-first---diseño-responsivo)
3. [Configuración de Tailwind](#configuración-de-tailwind)
4. [Sistema de Colores](#sistema-de-colores)
5. [Tipografía](#tipografía)
6. [Iconografía](#iconografía)
7. [Componentes Base](#componentes-base)
8. [Patrones de Layout](#patrones-de-layout)
9. [Estados y Feedback](#estados-y-feedback)
10. [Modo Oscuro](#modo-oscuro)
11. [Ejemplos de Componentes](#ejemplos-de-componentes)

---

## Filosofía de Diseño

### Integración con Angular 20

Este skill define **QUÉ** estilos usar. Para **CÓMO** estructurar el código Angular, seguir:

| Aspecto | Este Skill (UI) | Frontend Skill |
|---------|-----------------|----------------|
| Clases Tailwind | ✅ Definido aquí | Referencia aquí |
| Colores, tipografía | ✅ Definido aquí | Usa este skill |
| Estructura de archivos | Referencia allá | ✅ Definido allá |
| Signals, inject() | Referencia allá | ✅ Definido allá |
| @if/@for syntax | Referencia allá | ✅ Definido allá |

### Principios Fundamentales

| Principio | Descripción |
|-----------|-------------|
| **Mobile First** | Diseñar primero para móvil, luego escalar a desktop. Clases sin prefijo = mobile, usar `sm:`, `md:`, `lg:`, `xl:` para pantallas mayores |
| **Brutalist** | Bordes cuadrados (`rounded-none`), sin curvas suaves. Solo `rounded-full` para elementos circulares (avatares, badges de estado) |
| **Alto Contraste** | Negro sobre blanco, naranja como acento dominante |
| **Tipografía Bold** | Uso intensivo de `font-bold`, `font-black`, `uppercase`, `tracking-wider` |
| **Jerarquía Clara** | Tamaños de texto muy diferenciados, espaciado generoso |
| **Dark Mode Obligatorio** | Todos los componentes DEBEN tener variantes `dark:` |

### Identidad Visual

```
┌─────────────────────────────────────────────┐
│  ESTILO: Industrial / Deportivo / Premium   │
│  SENSACIÓN: Profesional, Energético, Limpio │
│  INSPIRACIÓN: Apps deportivas de élite      │
└─────────────────────────────────────────────┘
```

---

## Mobile First - Diseño Responsivo

### 📱 Filosofía Mobile First

> **Regla de Oro:** Todo componente DEBE funcionar perfectamente en móvil (320px+) ANTES de agregar estilos para desktop.

**Flujo de Desarrollo:**
```
1. Diseñar en móvil (sin breakpoints)
2. Probar en 320px - 640px
3. Agregar ajustes para tablet (sm:, md:)
4. Optimizar para desktop (lg:, xl:, 2xl:)
```

### 🎯 Breakpoints de Tailwind

| Breakpoint | Min Width | Dispositivo | Uso Principal |
|------------|-----------|-------------|---------------|
| **Default** | 0px | Mobile | Base (sin prefijo) |
| `sm:` | 640px | Large Mobile | Ajustes para móviles grandes |
| `md:` | 768px | Tablet | Layouts de 2 columnas |
| `lg:` | 1024px | Desktop | Sidebar + contenido |
| `xl:` | 1280px | Large Desktop | Contenido ancho |
| `2xl:` | 1536px | Extra Large | Márgenes generosos |

### ✅ Reglas Obligatorias

#### 1. **Clases Base = Mobile**
```html
<!-- ✅ CORRECTO: Base es mobile, escala a desktop -->
<div class="p-4 lg:p-8">
  <h1 class="text-2xl lg:text-4xl font-bold">Título</h1>
</div>

<!-- ❌ INCORRECTO: Empezar con desktop -->
<div class="p-8 sm:p-4">
  <h1 class="text-4xl sm:text-2xl">Título</h1>
</div>
```

#### 2. **Touch Targets Mínimos**
- Botones/enlaces: **Mínimo 44x44px** (área táctil)
- Inputs: **Mínimo 48px de altura**
- Checkboxes/radios: **Mínimo 24x24px**

```html
<!-- ✅ Touch-friendly -->
<button class="h-12 px-6 min-w-[120px]">Acción</button>
<input class="h-12 px-4" type="text">
```

#### 3. **Espaciado Vertical Generoso en Mobile**
```html
<!-- Mobile: más espacio vertical -->
<section class="space-y-6 lg:space-y-4">
  <div class="mb-6 lg:mb-4">...</div>
</section>
```

#### 4. **Stack en Mobile, Grid/Flex en Desktop**
```html
<!-- ✅ Vertical en mobile, horizontal en desktop -->
<div class="flex flex-col lg:flex-row gap-4">
  <aside class="w-full lg:w-64">Sidebar</aside>
  <main class="flex-1">Contenido</main>
</div>

<!-- ✅ 1 columna mobile, 2+ en desktop -->
<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
  <div>Card 1</div>
  <div>Card 2</div>
  <div>Card 3</div>
</div>
```

#### 5. **Ocultar/Mostrar Elementos**
```html
<!-- Hamburger solo en mobile -->
<button class="lg:hidden">
  <span class="material-symbols-outlined">menu</span>
</button>

<!-- Desktop nav oculto en mobile -->
<nav class="hidden lg:block">
  <ul>...</ul>
</nav>
```

### 📐 Patrones de Layout Responsivo

#### Layout Principal de App
```html
<div class="min-h-screen">
  <!-- Header: Full width en mobile, con sidebar en desktop -->
  <header class="h-16 border-b-4 border-black dark:border-white
                 lg:ml-64">
    <!-- Contenido -->
  </header>
  
  <!-- Sidebar: Drawer en mobile, fijo en desktop -->
  <aside class="fixed inset-y-0 left-0 w-64 
                transform -translate-x-full lg:translate-x-0
                transition-transform duration-300
                bg-white dark:bg-sidebar-dark
                border-r-4 border-black dark:border-white
                z-50">
    <!-- Nav -->
  </aside>
  
  <!-- Main: Full width en mobile, con margen en desktop -->
  <main class="pt-16 lg:ml-64 p-4 lg:p-8">
    <!-- Contenido -->
  </main>
</div>
```

#### Cards Responsivas
```html
<div class="bg-white dark:bg-surface-dark-alt
            border-4 border-black dark:border-white
            p-4 lg:p-6
            space-y-4 lg:space-y-6">
  
  <!-- Header: Stack en mobile, row en desktop -->
  <div class="flex flex-col lg:flex-row lg:items-center lg:justify-between
              gap-4 lg:gap-6">
    <h2 class="text-xl lg:text-2xl font-bold">Título</h2>
    <button class="w-full lg:w-auto">Acción</button>
  </div>
  
  <!-- Contenido -->
</div>
```

#### Formularios Responsivos
```html
<form class="space-y-4">
  <!-- 1 columna mobile, 2 en desktop -->
  <div class="grid grid-cols-1 lg:grid-cols-2 gap-4">
    <div>
      <label class="block text-sm font-bold mb-2">Campo 1</label>
      <input class="w-full h-12 px-4 border-2" type="text">
    </div>
    <div>
      <label class="block text-sm font-bold mb-2">Campo 2</label>
      <input class="w-full h-12 px-4 border-2" type="text">
    </div>
  </div>
  
  <!-- Botones: Stack mobile, inline desktop -->
  <div class="flex flex-col lg:flex-row gap-3 lg:gap-4">
    <button class="w-full lg:w-auto h-12 px-6">Guardar</button>
    <button class="w-full lg:w-auto h-12 px-6">Cancelar</button>
  </div>
</form>
```

#### Tablas Responsivas
```html
<!-- Mobile: Cards stacked -->
<div class="lg:hidden space-y-4">
  @for (item of items(); track item.id) {
    <div class="border-4 border-black dark:border-white p-4">
      <div class="font-bold mb-2">{{ item.name }}</div>
      <div class="text-sm space-y-1">
        <div><span class="font-bold">Estado:</span> {{ item.status }}</div>
        <div><span class="font-bold">Fecha:</span> {{ item.date }}</div>
      </div>
    </div>
  }
</div>

<!-- Desktop: Table normal -->
<div class="hidden lg:block overflow-x-auto">
  <table class="w-full">
    <thead>
      <tr class="border-b-4 border-black dark:border-white">
        <th class="text-left p-4 font-bold">Nombre</th>
        <th class="text-left p-4 font-bold">Estado</th>
        <th class="text-left p-4 font-bold">Fecha</th>
      </tr>
    </thead>
    <tbody>
      @for (item of items(); track item.id) {
        <tr class="border-b-2">
          <td class="p-4">{{ item.name }}</td>
          <td class="p-4">{{ item.status }}</td>
          <td class="p-4">{{ item.date }}</td>
        </tr>
      }
    </tbody>
  </table>
</div>
```

### 🎨 Tipografía Responsiva

```html
<!-- Títulos principales -->
<h1 class="text-3xl lg:text-5xl font-black">Hero Title</h1>
<h2 class="text-2xl lg:text-4xl font-bold">Section Title</h2>
<h3 class="text-xl lg:text-2xl font-bold">Subsection</h3>

<!-- Texto de cuerpo -->
<p class="text-base lg:text-lg">Párrafo normal</p>

<!-- Reducir padding en mobile -->
<section class="px-4 py-6 lg:px-8 lg:py-12">
  <div class="max-w-7xl mx-auto">
    <!-- Contenido limitado -->
  </div>
</section>
```

### 🚫 Anti-Patrones (NO HACER)

```html
<!-- ❌ Tamaños fijos en mobile -->
<div class="w-[800px]">Contenido</div>

<!-- ✅ Full width en mobile, limitado en desktop -->
<div class="w-full lg:w-[800px]">Contenido</div>

<!-- ❌ Scroll horizontal en mobile -->
<div class="flex space-x-4 w-max">Items</div>

<!-- ✅ Wrap en mobile -->
<div class="flex flex-wrap gap-4">Items</div>

<!-- ❌ Hover como único feedback en mobile -->
<button class="hover:bg-primary">Botón</button>

<!-- ✅ Estados claros (active, focus) -->
<button class="active:bg-primary-hover focus:ring-4">Botón</button>
```

### 📱 Testing Checklist

Antes de aprobar un componente:

- [ ] Funciona en **320px** (iPhone SE)
- [ ] Funciona en **375px** (iPhone 12/13)
- [ ] Funciona en **768px** (iPad portrait)
- [ ] Funciona en **1024px** (iPad landscape / Desktop pequeño)
- [ ] Funciona en **1920px** (Desktop estándar)
- [ ] Áreas táctiles >= 44px
- [ ] Sin scroll horizontal
- [ ] Texto legible sin zoom
- [ ] Imágenes responsive (sin overflow)

---

## Configuración de Tailwind

### tailwind.config.js

```javascript
tailwind.config = {
  darkMode: "class",
  theme: {
    extend: {
      colors: {
        // === COLORES PRIMARIOS ===
        "primary": "#FF6B00",
        "primary-hover": "#E65F00",
        "primary-light": "#FF8533",
        
        // === NEUTRALES ===
        "neutral-black": "#1A1A1A",
        "neutral-dark": "#111111",
        "neutral-gray": "#F4F4F4",
        
        // === SUPERFICIES DARK MODE ===
        "surface-dark": "#121212",
        "surface-dark-alt": "#1A1A1A",
        "sidebar-dark": "#000000",
        
        // === ESTADOS SEMÁNTICOS ===
        "success": "#22C55E",
        "success-light": "#DCFCE7",
        "warning": "#F59E0B",
        "warning-light": "#FEF3C7",
        "error": "#EF4444",
        "error-light": "#FEE2E2",
        "info": "#3B82F6",
        "info-light": "#DBEAFE",
      },
      fontFamily: {
        "display": ["Lexend", "sans-serif"]
      },
      // === BRUTALIST: SIN BORDES REDONDEADOS ===
      borderRadius: {
        "none": "0px",
        "DEFAULT": "0px",
        "sm": "0px",
        "md": "0px",
        "lg": "0px",
        "xl": "0px",
        "2xl": "0px",
        "full": "9999px"  // Solo para círculos
      },
    },
  },
}
```

### Estilos Base Globales

```css
/* styles.css o en <style> del componente raíz */

@import url('https://fonts.googleapis.com/css2?family=Lexend:wght@100..900&display=swap');
@import url('https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:wght,FILL@100..700,0..1&display=swap');

body {
  font-family: 'Lexend', sans-serif;
  @apply bg-white dark:bg-surface-dark text-neutral-black dark:text-white;
}

.material-symbols-outlined {
  font-variation-settings: 'FILL' 0, 'wght' 400, 'GRAD' 0, 'opsz' 24;
}

/* Iconos rellenos (para estados activos) */
.material-symbols-filled {
  font-variation-settings: 'FILL' 1, 'wght' 400, 'GRAD' 0, 'opsz' 24;
}
```

---

## Sistema de Colores

### Paleta Principal

| Nombre | Valor | Uso |
|--------|-------|-----|
| `primary` | `#FF6B00` | CTAs, enlaces activos, acentos, bordes destacados |
| `primary-hover` | `#E65F00` | Hover en botones primarios |
| `neutral-black` | `#1A1A1A` | Texto principal, fondos de botones secundarios |
| `neutral-gray` | `#F4F4F4` | Fondos de secciones, cards alternativas |

### Aplicación de Colores

```html
<!-- ✅ CORRECTO: Texto principal -->
<p class="text-neutral-black dark:text-white">Contenido</p>

<!-- ✅ CORRECTO: Texto secundario -->
<p class="text-gray-500 dark:text-gray-400">Subtítulo</p>

<!-- ✅ CORRECTO: Texto terciario/labels -->
<span class="text-gray-400 dark:text-gray-500">Label pequeño</span>

<!-- ✅ CORRECTO: Acento primario -->
<span class="text-primary">Destacado</span>

<!-- ✅ CORRECTO: Fondo de sección -->
<div class="bg-neutral-gray dark:bg-surface-dark-alt">...</div>

<!-- ✅ CORRECTO: Borde sutil -->
<div class="border border-gray-200 dark:border-zinc-800">...</div>
```

### Gradientes y Acentos

```html
<!-- Borde lateral de acento (muy usado en cards) -->
<div class="border-l-4 border-primary pl-6">
  <!-- Contenido con acento visual -->
</div>

<!-- Badge con fondo de primary transparente -->
<span class="bg-primary/10 text-primary px-3 py-1">
  Categoría
</span>
```

---

## Tipografía

### Escala Tipográfica

| Elemento | Clases Tailwind |
|----------|-----------------|
| **H1 - Títulos de página** | `text-4xl md:text-5xl font-black uppercase tracking-tight` |
| **H2 - Títulos de sección** | `text-2xl md:text-3xl font-bold tracking-tight` |
| **H3 - Títulos de card** | `text-xl font-bold uppercase tracking-tight` |
| **H4 - Subtítulos** | `text-lg font-semibold` |
| **Body** | `text-base font-medium` |
| **Body small** | `text-sm font-medium` |
| **Caption/Label** | `text-xs font-bold uppercase tracking-wider` |
| **Micro** | `text-[10px] font-black uppercase tracking-widest` |

### Ejemplos de Uso

```html
<!-- Título de página con etiqueta superior -->
<div class="flex flex-col gap-2">
  <span class="text-primary text-xs font-black uppercase tracking-[4px]">
    Panel de Jugador
  </span>
  <h1 class="text-4xl md:text-5xl font-black uppercase tracking-tight text-neutral-black dark:text-white">
    Explorar Torneos
  </h1>
  <p class="text-gray-500 dark:text-gray-400 text-lg font-medium">
    Descripción secundaria del contexto.
  </p>
</div>

<!-- Título de card -->
<h3 class="text-xl font-black uppercase tracking-tight group-hover:text-primary transition-colors">
  Summer Open 2024
</h3>

<!-- Labels de tabla -->
<th class="text-xs font-bold uppercase tracking-wider text-gray-500 dark:text-gray-400">
  Categoría
</th>
```

---

## Iconografía

### Configuración de Material Symbols

```html
<!-- Icono estándar -->
<span class="material-symbols-outlined">sports_tennis</span>

<!-- Icono con tamaño personalizado -->
<span class="material-symbols-outlined text-lg">calendar_month</span>
<span class="material-symbols-outlined text-xl">group</span>
<span class="material-symbols-outlined text-2xl">emoji_events</span>

<!-- Icono relleno (para estados activos) -->
<span class="material-symbols-outlined" style="font-variation-settings: 'FILL' 1">
  verified_user
</span>

<!-- Icono con color primary -->
<span class="material-symbols-outlined text-primary">location_on</span>
```

### Iconos Comunes en la App

| Contexto | Icono |
|----------|-------|
| Torneos | `emoji_events`, `trophy` |
| Calendario | `calendar_month`, `event` |
| Ubicación | `location_on` |
| Jugadores/Parejas | `group`, `groups`, `person` |
| Canchas | `table_view`, `grid_view` |
| Configuración | `settings` |
| Búsqueda | `search` |
| Añadir | `add`, `add_circle` |
| Editar | `edit` |
| Eliminar | `delete` |
| Notificaciones | `notifications` |
| Cerrar sesión | `logout` |
| Navegación | `chevron_right`, `chevron_left`, `expand_more` |

---

## Componentes Base

### Botones

#### Botón Primario
```html
<button class="
  bg-primary hover:bg-primary-hover 
  text-white 
  px-6 py-3 
  text-xs font-black uppercase tracking-widest 
  transition-all 
  shadow-lg shadow-primary/20
">
  Inscribirse Ahora
</button>
```

#### Botón Secundario (Negro)
```html
<button class="
  bg-neutral-black dark:bg-white 
  text-white dark:text-black 
  px-6 py-3 
  text-xs font-black uppercase tracking-widest 
  hover:bg-primary 
  transition-all
">
  Ver Detalles
</button>
```

#### Botón Outline
```html
<button class="
  border border-gray-200 dark:border-zinc-800 
  bg-white dark:bg-transparent 
  text-neutral-black dark:text-white 
  px-5 py-2.5 
  text-sm font-bold 
  hover:border-primary hover:text-primary 
  transition-all
">
  Cancelar
</button>
```

#### Botón Icono
```html
<button class="
  p-2 
  text-gray-400 
  hover:text-primary 
  transition-colors
">
  <span class="material-symbols-outlined">edit</span>
</button>
```

#### Botón con Icono y Texto
```html
<button class="
  flex items-center gap-2 
  bg-primary hover:bg-primary-hover 
  text-white 
  px-5 py-2.5 
  text-sm font-bold 
  transition-all
">
  <span class="material-symbols-outlined text-lg">add</span>
  <span>Agregar Partido</span>
</button>
```

### Inputs

#### Input de Texto Estándar
```html
<div class="flex flex-col gap-2">
  <label class="text-sm font-semibold text-gray-700 dark:text-gray-300">
    Nombre del Torneo
  </label>
  <input 
    type="text"
    class="
      w-full px-4 py-3 
      bg-white dark:bg-zinc-900 
      border border-gray-200 dark:border-zinc-800 
      text-neutral-black dark:text-white 
      placeholder:text-gray-400 
      focus:ring-2 focus:ring-primary/20 focus:border-primary 
      outline-none transition-all
    "
    placeholder="Ej: Summer Open 2024"
  />
</div>
```

#### Input con Icono (Búsqueda)
```html
<div class="flex items-stretch bg-white dark:bg-zinc-900 border border-gray-200 dark:border-zinc-800">
  <div class="flex items-center justify-center px-4 text-gray-400 border-r border-gray-200 dark:border-zinc-800">
    <span class="material-symbols-outlined">search</span>
  </div>
  <input 
    type="text"
    class="
      flex-1 px-4 py-3 
      bg-transparent 
      border-none 
      text-neutral-black dark:text-white 
      placeholder:text-gray-400 
      focus:ring-0 focus:outline-none
    "
    placeholder="Buscar torneos, clubes o ciudades..."
  />
</div>
```

#### Select / Dropdown Button
```html
<button class="
  flex items-center justify-between gap-3 
  bg-white dark:bg-zinc-900 
  border border-gray-200 dark:border-zinc-800 
  px-5 py-3 
  min-w-[180px]
  hover:border-primary 
  transition-all
">
  <span class="text-xs font-bold uppercase tracking-wider text-neutral-black dark:text-white">
    Ubicación: Madrid
  </span>
  <span class="material-symbols-outlined text-sm text-gray-400">expand_more</span>
</button>
```

### Cards

#### Card de Torneo (con imagen)
```html
<div class="
  flex flex-col 
  bg-white dark:bg-surface-dark-alt 
  border border-gray-200 dark:border-zinc-800 
  overflow-hidden 
  group 
  hover:border-primary 
  transition-all duration-300
">
  <!-- Imagen -->
  <div class="relative overflow-hidden">
    <div 
      class="w-full aspect-video bg-cover bg-center transition-transform duration-500 group-hover:scale-105"
      style="background-image: url('imagen.jpg');"
    ></div>
    <!-- Badge superior -->
    <div class="absolute top-0 left-0 bg-primary text-white text-[10px] font-black uppercase tracking-widest px-3 py-2">
      Inscripciones Abiertas
    </div>
    <!-- Badge precio -->
    <div class="absolute bottom-4 right-4 bg-white/90 dark:bg-black/90 backdrop-blur px-3 py-1 text-[10px] font-black uppercase">
      Desde 25€
    </div>
  </div>
  
  <!-- Contenido -->
  <div class="p-6 flex flex-col flex-1 border-l-4 border-primary">
    <h3 class="text-xl font-black uppercase tracking-tight mb-4 group-hover:text-primary transition-colors">
      Summer Open 2024
    </h3>
    
    <!-- Metadatos -->
    <div class="space-y-3 mb-6">
      <div class="flex items-center gap-3 text-gray-500 dark:text-gray-400 text-sm">
        <span class="material-symbols-outlined text-lg text-primary">location_on</span>
        <span class="font-medium">Padel Pro Center, Madrid</span>
      </div>
      <div class="flex items-center gap-3 text-gray-500 dark:text-gray-400 text-sm">
        <span class="material-symbols-outlined text-lg text-primary">calendar_month</span>
        <span class="font-medium">12 Oct - 14 Oct</span>
      </div>
    </div>
    
    <!-- Tags -->
    <div class="flex flex-wrap gap-2 mb-6">
      <span class="px-3 py-1 bg-neutral-gray dark:bg-zinc-800 text-[10px] font-bold uppercase tracking-wider">
        Masculino A
      </span>
      <span class="px-3 py-1 bg-primary/10 text-primary text-[10px] font-bold uppercase tracking-wider">
        Avanzado
      </span>
    </div>
    
    <!-- CTA -->
    <button class="w-full py-4 mt-auto bg-neutral-black dark:bg-white text-white dark:text-black text-xs font-black uppercase tracking-widest hover:bg-primary dark:hover:bg-primary dark:hover:text-white transition-all">
      Inscribirse Ahora
    </button>
  </div>
</div>
```

#### Card de Estadística (KPI)
```html
<div class="
  flex flex-col gap-2 
  p-6 
  bg-white dark:bg-surface-dark-alt 
  border border-gray-200 dark:border-zinc-800
">
  <p class="text-gray-500 dark:text-gray-400 text-xs font-bold uppercase tracking-wider">
    Total Categorías
  </p>
  <div class="flex items-baseline gap-3">
    <p class="text-neutral-black dark:text-white text-4xl font-black">8</p>
    <p class="text-primary text-sm font-bold">+1 esta semana</p>
  </div>
</div>

<!-- Variante con borde de acento -->
<div class="
  flex flex-col gap-2 
  p-6 
  bg-white dark:bg-surface-dark-alt 
  border border-gray-200 dark:border-zinc-800 
  border-l-4 border-l-success
">
  <p class="text-gray-500 dark:text-gray-400 text-xs font-bold uppercase tracking-wider">
    Canchas Activas
  </p>
  <div class="flex items-baseline gap-3">
    <p class="text-neutral-black dark:text-white text-4xl font-black">10</p>
    <p class="text-success text-sm font-bold">+2 esta semana</p>
  </div>
</div>
```

### Tablas

```html
<div class="overflow-hidden border border-gray-200 dark:border-zinc-800 bg-white dark:bg-surface-dark-alt">
  <table class="w-full text-left">
    <thead class="bg-gray-50 dark:bg-zinc-800/50">
      <tr>
        <th class="px-6 py-4 text-xs font-bold uppercase tracking-wider text-gray-500 dark:text-gray-400">
          Categoría
        </th>
        <th class="px-6 py-4 text-xs font-bold uppercase tracking-wider text-gray-500 dark:text-gray-400">
          Modalidad
        </th>
        <th class="px-6 py-4 text-xs font-bold uppercase tracking-wider text-gray-500 dark:text-gray-400 text-right">
          Precio
        </th>
        <th class="px-6 py-4 text-xs font-bold uppercase tracking-wider text-gray-500 dark:text-gray-400 text-center">
          Acciones
        </th>
      </tr>
    </thead>
    <tbody class="divide-y divide-gray-200 dark:divide-zinc-800">
      <tr class="hover:bg-primary/5 dark:hover:bg-primary/10 transition-colors">
        <td class="px-6 py-5">
          <div class="flex flex-col">
            <span class="text-neutral-black dark:text-white font-semibold">1ª Categoría</span>
            <span class="text-gray-500 text-xs">Nivel Avanzado</span>
          </div>
        </td>
        <td class="px-6 py-5">
          <span class="px-2.5 py-1 bg-gray-100 dark:bg-zinc-800 text-neutral-black dark:text-white text-xs font-bold uppercase tracking-wider">
            Masculino
          </span>
        </td>
        <td class="px-6 py-5 text-right font-medium text-neutral-black dark:text-white">
          50,00 €
        </td>
        <td class="px-6 py-5">
          <div class="flex justify-center gap-2">
            <button class="p-2 text-gray-400 hover:text-primary transition-colors">
              <span class="material-symbols-outlined">edit</span>
            </button>
            <button class="p-2 text-gray-400 hover:text-error transition-colors">
              <span class="material-symbols-outlined">delete</span>
            </button>
          </div>
        </td>
      </tr>
    </tbody>
  </table>
</div>
```

### Navegación / Tabs

```html
<div class="flex border-b border-gray-200 dark:border-zinc-800 gap-8">
  <!-- Tab Activo -->
  <a 
    href="#" 
    class="flex items-center pb-3 pt-4 border-b-[3px] border-primary text-primary"
  >
    <span class="text-sm font-bold tracking-wide">Todas</span>
  </a>
  
  <!-- Tab Inactivo -->
  <a 
    href="#" 
    class="flex items-center pb-3 pt-4 border-b-[3px] border-transparent text-gray-500 dark:text-gray-400 hover:text-neutral-black dark:hover:text-white transition-colors"
  >
    <span class="text-sm font-bold tracking-wide">Masculino</span>
  </a>
</div>
```

### Badges / Status Indicators

```html
<!-- Estado: Abierto -->
<span class="
  inline-flex items-center gap-1.5 
  px-3 py-1 
  bg-success-light dark:bg-success/20 
  text-success 
  text-xs font-bold
">
  <span class="w-2 h-2 rounded-full bg-success"></span>
  Abierto
</span>

<!-- Estado: Completo -->
<span class="
  inline-flex items-center gap-1.5 
  px-3 py-1 
  bg-warning-light dark:bg-warning/20 
  text-warning 
  text-xs font-bold
">
  <span class="w-2 h-2 rounded-full bg-warning"></span>
  Completo
</span>

<!-- Estado: Cerrado -->
<span class="
  inline-flex items-center gap-1.5 
  px-3 py-1 
  bg-error-light dark:bg-error/20 
  text-error 
  text-xs font-bold
">
  <span class="w-2 h-2 rounded-full bg-error"></span>
  Cerrado
</span>

<!-- Rol / Categoría -->
<span class="
  inline-flex items-center 
  px-3 py-1 
  bg-primary/10 
  text-primary 
  text-xs font-bold uppercase tracking-wider 
  border border-primary/20
">
  Director
</span>

<!-- Tag neutral -->
<span class="
  px-3 py-1 
  bg-neutral-gray dark:bg-zinc-800 
  text-neutral-black dark:text-white 
  text-[10px] font-bold uppercase tracking-wider
">
  Mixto C
</span>
```

### Progress Bars

```html
<div class="flex items-center gap-3">
  <div class="flex-1 max-w-[120px] overflow-hidden bg-gray-200 dark:bg-zinc-800">
    <div 
      class="h-2 bg-primary" 
      style="width: 75%;"
    ></div>
  </div>
  <span class="text-neutral-black dark:text-white text-sm font-bold">24/32</span>
</div>
```

---

## Patrones de Layout

### Container Principal

```html
<main class="flex-1 max-w-[1200px] mx-auto w-full px-4 md:px-10 py-12">
  <!-- Contenido -->
</main>
```

### Header de Página

```html
<div class="flex flex-wrap justify-between items-end gap-6 mb-10 border-l-4 border-primary pl-6">
  <div class="flex flex-col gap-2">
    <span class="text-primary text-xs font-black uppercase tracking-[4px]">
      Panel de Jugador
    </span>
    <h1 class="text-4xl md:text-5xl font-black uppercase tracking-tight text-neutral-black dark:text-white">
      Explorar Torneos
    </h1>
    <p class="text-gray-500 dark:text-gray-400 text-lg font-medium max-w-2xl">
      Descripción de la sección.
    </p>
  </div>
  <div class="flex gap-3">
    <!-- Acciones -->
  </div>
</div>
```

### Sidebar (Admin)

```html
<aside class="w-64 flex-shrink-0 bg-black text-white flex flex-col">
  <!-- Logo -->
  <div class="p-6 flex items-center gap-3">
    <div class="bg-primary p-2 text-white">
      <span class="material-symbols-outlined">sports_tennis</span>
    </div>
    <div>
      <h1 class="text-xs font-bold uppercase tracking-wider text-gray-400">Panel Admin</h1>
      <p class="text-sm text-white font-medium">Padel Pro 2024</p>
    </div>
  </div>
  
  <!-- Navegación -->
  <nav class="flex flex-col gap-1 px-4">
    <!-- Item activo -->
    <a href="#" class="flex items-center gap-3 px-3 py-2.5 bg-primary text-white">
      <span class="material-symbols-outlined">calendar_month</span>
      <span class="text-sm font-semibold">Calendario</span>
    </a>
    
    <!-- Item inactivo -->
    <a href="#" class="flex items-center gap-3 px-3 py-2.5 text-gray-400 hover:bg-white/10 hover:text-white transition-colors">
      <span class="material-symbols-outlined">settings</span>
      <span class="text-sm font-medium">Configuración</span>
    </a>
  </nav>
  
  <!-- Footer con usuario -->
  <div class="mt-auto p-4 border-t border-white/10">
    <div class="flex items-center gap-3">
      <div class="size-10 rounded-full bg-cover bg-center border-2 border-primary" style="background-image: url('avatar.jpg');"></div>
      <div class="flex-1">
        <p class="text-sm font-semibold">Alex Rivera</p>
        <p class="text-xs text-gray-500">Super Admin</p>
      </div>
      <span class="material-symbols-outlined text-gray-500 hover:text-primary cursor-pointer">logout</span>
    </div>
  </div>
</aside>
```

### Breadcrumbs

```html
<nav class="flex items-center gap-2 mb-6 text-sm">
  <a href="#" class="text-gray-500 dark:text-gray-400 hover:text-primary transition-colors">
    Admin
  </a>
  <span class="material-symbols-outlined text-gray-300 dark:text-zinc-700 text-sm">chevron_right</span>
  <a href="#" class="text-gray-500 dark:text-gray-400 hover:text-primary transition-colors">
    Configuración
  </a>
  <span class="material-symbols-outlined text-gray-300 dark:text-zinc-700 text-sm">chevron_right</span>
  <span class="text-neutral-black dark:text-white font-semibold">
    Permisos
  </span>
</nav>
```

### Grid de Cards

```html
<!-- Grid responsive 1-2-3 columnas -->
<div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 md:gap-8">
  <!-- Cards -->
</div>

<!-- Grid de KPIs -->
<div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4 md:gap-6">
  <!-- KPI Cards -->
</div>
```

---

## Estados y Feedback

### Hover States

```html
<!-- Card hover -->
<div class="hover:border-primary transition-all duration-300">

<!-- Botón hover con sombra -->
<button class="hover:bg-primary-hover shadow-lg shadow-primary/20 hover:shadow-primary/30 transition-all">

<!-- Row de tabla hover -->
<tr class="hover:bg-primary/5 dark:hover:bg-primary/10 transition-colors">

<!-- Link hover -->
<a class="hover:text-primary transition-colors">

<!-- Icono hover -->
<button class="text-gray-400 hover:text-primary transition-colors">
```

### Focus States

```html
<!-- Input focus -->
<input class="
  focus:ring-2 focus:ring-primary/20 focus:border-primary 
  outline-none transition-all
"/>

<!-- Botón focus (accesibilidad) -->
<button class="
  focus:outline-none focus:ring-2 focus:ring-primary focus:ring-offset-2
">
```

### Estados de Carga

```html
<!-- Skeleton loader para card -->
<div class="animate-pulse">
  <div class="aspect-video bg-gray-200 dark:bg-zinc-800"></div>
  <div class="p-6 space-y-4">
    <div class="h-6 bg-gray-200 dark:bg-zinc-800 w-3/4"></div>
    <div class="h-4 bg-gray-200 dark:bg-zinc-800 w-1/2"></div>
    <div class="h-4 bg-gray-200 dark:bg-zinc-800 w-2/3"></div>
  </div>
</div>

<!-- Spinner -->
<div class="animate-spin size-6 border-2 border-primary border-t-transparent rounded-full"></div>
```

---

## Modo Oscuro

### Reglas Obligatorias

Todo componente DEBE incluir variantes `dark:` para:

| Propiedad | Light | Dark |
|-----------|-------|------|
| **Background principal** | `bg-white` | `dark:bg-surface-dark` |
| **Background alternativo** | `bg-neutral-gray` | `dark:bg-surface-dark-alt` |
| **Background sidebar** | `bg-white` | `dark:bg-black` |
| **Texto principal** | `text-neutral-black` | `dark:text-white` |
| **Texto secundario** | `text-gray-500` | `dark:text-gray-400` |
| **Bordes** | `border-gray-200` | `dark:border-zinc-800` |
| **Inputs background** | `bg-white` | `dark:bg-zinc-900` |

### Patrón de Aplicación

```html
<!-- ✅ SIEMPRE incluir ambos modos -->
<div class="bg-white dark:bg-surface-dark">
  <h1 class="text-neutral-black dark:text-white">Título</h1>
  <p class="text-gray-500 dark:text-gray-400">Subtítulo</p>
  <div class="border border-gray-200 dark:border-zinc-800">
    <!-- Contenido -->
  </div>
</div>

<!-- ❌ NUNCA dejar sin dark mode -->
<div class="bg-white">
  <h1 class="text-black">Título</h1>
</div>
```

---

## Ejemplos de Componentes

> **⚠️ Nota:** Los siguientes ejemplos muestran la estructura completa para referencia.
> En producción, usar archivos separados según [Frontend Architecture Skill](./skill-frontend.md).

### Componente Angular: Tournament Card

**Estructura de archivos:**
```
tournament-card/
├── tournament-card.component.ts
├── tournament-card.component.html
└── tournament-card.component.css
```

```typescript
// tournament-card.component.ts
import { Component, ChangeDetectionStrategy, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Tournament } from '../../models/tournament.model';

@Component({
  selector: 'app-tournament-card',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './tournament-card.component.html',
  styleUrl: './tournament-card.component.css'
})
export class TournamentCardComponent {
  // Usar input() signal en lugar de @Input
  tournament = input.required<Tournament>();
  
  // Usar output() signal en lugar de @Output
  registerClick = output<Tournament>();
  
  onRegister(): void {
    this.registerClick.emit(this.tournament());
  }
}
```

```html
<!-- tournament-card.component.html -->
<article class="
  flex flex-col 
  bg-white dark:bg-surface-dark-alt 
  border border-gray-200 dark:border-zinc-800 
  overflow-hidden 
  group 
  hover:border-primary 
  transition-all duration-300
">
  <!-- Image Section -->
  <div class="relative overflow-hidden">
    <div 
      class="w-full aspect-video bg-cover bg-center transition-transform duration-500 group-hover:scale-105"
      [style.background-image]="'url(' + tournament().imageUrl + ')'"
    ></div>
    
    @if (tournament().status === 'open') {
      <div class="absolute top-0 left-0 bg-primary text-white text-[10px] font-black uppercase tracking-widest px-3 py-2">
        Open for Registration
      </div>
    }
    
    <div class="absolute bottom-4 right-4 bg-white/90 dark:bg-black/90 backdrop-blur px-3 py-1 text-[10px] font-black uppercase">
      From {{ tournament().price }}€
    </div>
  </div>
  
  <!-- Content Section -->
  <div class="p-6 flex flex-col flex-1 border-l-4 border-primary">
    <h3 class="text-xl font-black uppercase tracking-tight mb-4 group-hover:text-primary transition-colors">
      {{ tournament().name }}
    </h3>
    
    <div class="space-y-3 mb-6">
      <div class="flex items-center gap-3 text-gray-500 dark:text-gray-400 text-sm">
        <span class="material-symbols-outlined text-lg text-primary">location_on</span>
        <span class="font-medium">{{ tournament().location }}</span>
      </div>
      <div class="flex items-center gap-3 text-gray-500 dark:text-gray-400 text-sm">
        <span class="material-symbols-outlined text-lg text-primary">calendar_month</span>
        <span class="font-medium">{{ tournament().dates }}</span>
      </div>
      <div class="flex items-center gap-3 text-gray-500 dark:text-gray-400 text-sm">
        <span class="material-symbols-outlined text-lg text-primary">group</span>
        <span class="font-medium">{{ tournament().registeredPairs }}/{{ tournament().maxPairs }} Pairs</span>
      </div>
    </div>
    
    <div class="flex flex-wrap gap-2 mb-6">
      @for (category of tournament().categories; track category) {
        <span class="px-3 py-1 bg-neutral-gray dark:bg-zinc-800 text-[10px] font-bold uppercase tracking-wider">
          {{ category }}
        </span>
      }
    </div>
    
    <button 
      (click)="onRegister()"
      class="
        w-full py-4 mt-auto 
        bg-neutral-black dark:bg-white 
        text-white dark:text-black 
        text-xs font-black uppercase tracking-widest 
        hover:bg-primary dark:hover:bg-primary dark:hover:text-white 
        transition-all
      "
    >
      Register Now
    </button>
  </div>
</article>
```

```css
/* tournament-card.component.css */
/* Tailwind handles all styling - file can be empty or contain @apply for complex cases */
```
```

### Componente Angular: Status Badge

**Estructura de archivos:**
```
status-badge/
├── status-badge.component.ts
├── status-badge.component.html
└── status-badge.component.css
```

```typescript
// status-badge.component.ts
import { Component, ChangeDetectionStrategy, input, computed } from '@angular/core';
import { CommonModule } from '@angular/common';

type BadgeStatus = 'open' | 'full' | 'closed' | 'maintenance';

@Component({
  selector: 'app-status-badge',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './status-badge.component.html',
  styleUrl: './status-badge.component.css'
})
export class StatusBadgeComponent {
  // Usar input() signal
  status = input<BadgeStatus>('open');
  
  private statusConfig: Record<BadgeStatus, { container: string; dot: string; label: string }> = {
    open: {
      container: 'bg-success-light dark:bg-success/20 text-success',
      dot: 'bg-success',
      label: 'Open'
    },
    full: {
      container: 'bg-warning-light dark:bg-warning/20 text-warning',
      dot: 'bg-warning',
      label: 'Full'
    },
    closed: {
      container: 'bg-error-light dark:bg-error/20 text-error',
      dot: 'bg-error',
      label: 'Closed'
    },
    maintenance: {
      container: 'bg-info-light dark:bg-info/20 text-info',
      dot: 'bg-info',
      label: 'Maintenance'
    }
  };
  
  // Usar computed() para valores derivados
  statusClasses = computed(() => this.statusConfig[this.status()].container);
  dotClass = computed(() => this.statusConfig[this.status()].dot);
  label = computed(() => this.statusConfig[this.status()].label);
}
```

```html
<!-- status-badge.component.html -->
<span 
  class="inline-flex items-center gap-1.5 px-3 py-1 text-xs font-bold"
  [ngClass]="statusClasses()"
>
  <span class="w-2 h-2 rounded-full" [ngClass]="dotClass()"></span>
  {{ label() }}
</span>
```
```

---

## ✅ Checklist de Validación UI

Antes de hacer merge de cualquier componente UI, verificar:

### Estilos (Este Skill)
- [ ] Usa colores del sistema (`primary`, `neutral-black`, etc.)
- [ ] Tipografía sigue la escala definida
- [ ] Bordes son cuadrados (excepto círculos con `rounded-full`)
- [ ] Incluye variantes `dark:` para todos los colores
- [ ] Usa Material Symbols Outlined para iconos
- [ ] Estados hover tienen `transition-colors` o `transition-all`
- [ ] Espaciado usa valores estándar de Tailwind (4, 6, 8, etc.)
- [ ] Textos de botones/labels usan `uppercase tracking-wider`
- [ ] Cards tienen borde sutil y hover state con `border-primary`

### Arquitectura ([Frontend Skill](./skill-frontend.md))
- [ ] Archivos separados: `.ts`, `.html`, `.css`
- [ ] Usa `input()` / `output()` signals (no `@Input` / `@Output`)
- [ ] Usa `computed()` para valores derivados
- [ ] `ChangeDetectionStrategy.OnPush` configurado
- [ ] Usa `@if` / `@for` (no `*ngIf` / `*ngFor`)
- [ ] Código en inglés

---

## 🚫 Anti-patrones a Evitar

### Estilos (Este Skill)
```html
<!-- ❌ NO usar bordes redondeados (excepto full para círculos) -->
<div class="rounded-lg">...</div>

<!-- ❌ NO usar colores hardcodeados -->
<div class="bg-[#FF6B00]">...</div>
<span class="text-[#333]">...</span>

<!-- ❌ NO olvidar dark mode -->
<div class="bg-white text-black">...</div>

<!-- ❌ NO usar tipografías no definidas -->
<p class="font-sans">...</p>

<!-- ❌ NO usar iconos que no sean Material Symbols -->
<svg><!-- custom icon --></svg>

<!-- ❌ NO usar hover sin transiciones -->
<button class="hover:bg-primary">...</button>

<!-- ✅ CORRECTO -->
<button class="hover:bg-primary transition-colors">...</button>
```

### Arquitectura ([Frontend Skill](./skill-frontend.md))
```typescript
// ❌ NO usar template inline
@Component({
  template: `<div>...</div>`  // PROHIBIDO
})

// ❌ NO usar @Input/@Output decoradores
@Input() data!: Data;  // PROHIBIDO
@Output() clicked = new EventEmitter();  // PROHIBIDO

// ✅ CORRECTO - Usar signals
data = input.required<Data>();
clicked = output<void>();

// ❌ NO usar getters para valores derivados
get fullName() { return this.first + this.last; }  // PROHIBIDO

// ✅ CORRECTO - Usar computed()
fullName = computed(() => this.first() + ' ' + this.last());
```

---

## 📚 Referencias

| Skill | Propósito |
|-------|----------|
| **Este skill** | Colores, tipografía, iconos, componentes UI, dark mode |
| [Frontend Architecture](./skill-frontend.md) | Estructura, signals, patterns, arquitectura Angular 20 |

---

> **Última actualización:** Enero 2026  
> **Versión:** 1.1  
> **Stack:** Angular 20 + Tailwind CSS 4.x
