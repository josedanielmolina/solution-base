# Skill: Frontend Architecture (Angular 20 + Tailwind CSS)

**⚠️ SEPARACIÓN DE ARCHIVOS OBLIGATORIA:** Todos los componentes deben tener archivos separados `.ts`, `.html` y `.css`. NUNCA usar templates o estilos inline.

---

## 1. ESTRUCTURA DEL PROYECTO

```
src/app/
├── core/                      # Servicios singleton, Guards, Interceptors, Models globales
│   ├── services/
│   ├── guards/
│   ├── interceptors/
│   ├── models/
│   ├── utils/
│   └── constants/
├── shared/                    # Componentes, Pipes, Directivas reutilizables
│   ├── components/
│   ├── pipes/
│   └── directives/
├── features/                  # Módulos de características (lazy-loaded)
│   └── admin/                 # Feature principal
│       ├── tournament/
│       │   ├── pages/
│       │   │   ├── tournament-dashboard/
│       │   │   │   ├── tournament-dashboard.page.ts
│       │   │   │   ├── tournament-dashboard.page.html
│       │   │   │   └── tournament-dashboard.page.css
│       │   │   └── tournament-config/
│       │   │       ├── tournament-config.page.ts
│       │   │       ├── tournament-config.page.html
│       │   │       └── tournament-config.page.css
│       │   ├── services/
│       │   │   └── admin-tournament.service.ts
│       │   └── models/
│       │       └── admin-tournament.model.ts
│       ├── matches/
│       │   ├── pages/
│       │   │   ├── match-calendar/
│       │   │   │   ├── match-calendar.page.ts
│       │   │   │   ├── match-calendar.page.html
│       │   │   │   └── match-calendar.page.css
│       │   │   └── match-form/
│       │   │       ├── match-form.page.ts
│       │   │       ├── match-form.page.html
│       │   │       └── match-form.page.css
│       │   ├── components/
│       │   │   ├── match-card.component.ts
│       │   │   ├── match-card.component.html
│       │   │   └── match-card.component.css
│       │   ├── services/
│       │   │   └── match.service.ts
│       │   └── models/
│       │       └── match.model.ts
│       ├── courts/
│       │   ├── pages/
│       │   │   └── court-list/
│       │   │       ├── court-list.page.ts
│       │   │       ├── court-list.page.html
│       │   │       └── court-list.page.css
│       │   ├── services/
│       │   │   └── court.service.ts
│       │   └── models/
│       │       └── court.model.ts
│       ├── categories/
│       │   ├── pages/
│       │   │   └── category-list/
│       │   │       ├── category-list.page.ts
│       │   │       ├── category-list.page.html
│       │   │       └── category-list.page.css
│       │   ├── services/
│       │   │   └── category.service.ts
│       │   └── models/
│       │       └── category.model.ts
│       ├── administrators/
│       │   ├── pages/
│       │   │   └── admin-list/
│       │   │       ├── admin-list.page.ts
│       │   │       ├── admin-list.page.html
│       │   │       └── admin-list.page.css
│       │   ├── services/
│       │   │   └── administrator.service.ts
│       │   └── models/
│       │       └── administrator.model.ts
│       └── admin.routes.ts
├── layouts/                   # Layouts de la aplicación
└── environments/              # Configuración por ambiente
```

---

## 2. PATRONES CLAVE

**Standalone + OnPush**
```typescript
@Component({ selector: 'app-user-list', standalone: true, changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './user-list.component.html', styleUrl: './user-list.component.css' })
```

**Signals + inject()**
```typescript
users = signal<User[]>([]); activeUsers = computed(() => this.users().filter(u => u.isActive));
private service = inject(UserService);
```

**Input/Output Signals:** `user = input.required<User>(); userSelected = output<User>(); isExpanded = model<boolean>(false);`

**Control Flow:** `@if (loading()) { <div>Loading</div> } @for (user of users(); track user.id) { ... } @defer (on viewport) { <app-heavy /> }`

**Guards/Interceptors:** `export const authGuard: CanActivateFn = () => inject(AuthService).isLoggedIn();`

**SSR:** `bootstrapApplication(AppComponent, { providers: [provideClientHydration(withEventReplay()), provideExperimentalZonelessChangeDetection()] });`

**Path Aliases:** `"paths": { "@core/*": ["src/app/core/*"], "@shared/*": ["src/app/shared/*"], "@features/*": ["src/app/features/*"] }`

---

## 3. CONVENCIONES & REGLAS

**Nombrado:** Componente: `user-list.ts` | Servicio: `user.ts` | Guard: `auth.guard.ts` | Model: `user.model.ts` | Interface: `User`, `CreateUserDto` | Constante: `API_BASE_URL`

**Reglas Obligatorias:**
1. Standalone + OnPush + Archivos separados (.ts/.html/.css) SIEMPRE
2. Signals (NO BehaviorSubject) + input()/output()/model() (NO @Input/@Output)
3. inject() SIEMPRE (NO constructor injection)
4. @if/@for/@switch (NO *ngIf/*ngFor) + @defer + track en @for
5. Reactive Forms + Lazy loading + Guards/Interceptors funcionales
6. Encapsular librerías de terceros en servicios
7. Tailwind CSS
8. TypeScript strict + Código en inglés

---

## 4. CHECKLIST

- [ ] Standalone + OnPush + Archivos separados | Signals + inject()
- [ ] input()/output()/model() | @if/@for/@switch + @defer + track
- [ ] Reactive Forms + Lazy loading | Guards/Interceptors funcionales
- [ ] Librerías encapsuladas | Tailwind + dark: | TypeScript strict

---

## 5. TECNOLOGÍAS

Angular 20 | TypeScript 5.5+ (strict) | Tailwind CSS 4.x | Signals API | Esbuild/Vite | Zoneless + SSR + Event Replay

