# Skill: Frontend Architecture (Angular 20)

## Descripción
Este skill define las reglas de arquitectura, patrones y convenciones para el desarrollo frontend con Angular 20 que TODO el código nuevo debe seguir.

**⚠️ IMPORTANTE - SEPARACIÓN DE ARCHIVOS:**
- **TODOS los componentes deben tener archivos separados: .ts, .html y .css**
- **NUNCA usar templates o estilos inline en código de producción**
- En este documento, algunos ejemplos muestran templates inline solo con **fines didácticos** para facilitar la lectura
- En código real, siempre usar `templateUrl` y `styleUrl` apuntando a archivos separados

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
│   └── [feature-name]/
│       ├── components/
│       ├── services/
│       └── models/
├── layouts/                   # Layouts de la aplicación
└── environments/              # Configuración por ambiente
```

---

## 2. SEPARACIÓN DE ARCHIVOS (OBLIGATORIO)

**CADA componente DEBE tener sus archivos HTML, CSS y TypeScript separados.**

```typescript
// ✅ CORRECTO - Archivos separados
// user-list.component.ts
@Component({
  selector: 'app-user-list',
  standalone: true,
  templateUrl: './user-list.component.html',  // ✅ Archivo HTML separado
  styleUrl: './user-list.component.css'       // ✅ Archivo CSS separado
})
export class UserListComponent {}

// user-list.component.html
<div class="user-list">
  <h1>Users</h1>
  <!-- HTML aquí -->
</div>

// user-list.component.css
.user-list {
  padding: 20px;
}

// ❌ INCORRECTO - Template o estilos inline
@Component({
  selector: 'app-user-list',
  standalone: true,
  template: `
    <div class="user-list">
      <h1>Users</h1>
    </div>
  `,  // ❌ NUNCA usar template inline
  styles: [`
    .user-list { padding: 20px; }
  `]  // ❌ NUNCA usar styles inline
})
export class UserListComponent {}
```

**Estructura de archivos por componente:**
```
user-list/
├── user-list.component.ts      # Lógica del componente
├── user-list.component.html    # Template HTML
└── user-list.component.css     # Estilos CSS
```

---

## 2. SEPARACIÓN DE ARCHIVOS (OBLIGATORIO)

**CADA componente DEBE tener sus archivos HTML, CSS y TypeScript separados.**

```typescript
// ✅ CORRECTO - Archivos separados
// user-list.component.ts
@Component({
  selector: 'app-user-list',
  standalone: true,
  templateUrl: './user-list.component.html',  // ✅ Archivo HTML separado
  styleUrl: './user-list.component.css'       // ✅ Archivo CSS separado
})
export class UserListComponent {}

// user-list.component.html
<div class="user-list">
  <h1>Users</h1>
  <!-- HTML aquí -->
</div>

// user-list.component.css
.user-list {
  padding: 20px;
}

// ❌ INCORRECTO - Template o estilos inline
@Component({
  selector: 'app-user-list',
  standalone: true,
  template: `
    <div class="user-list">
      <h1>Users</h1>
    </div>
  `,  // ❌ NUNCA usar template inline
  styles: [`
    .user-list { padding: 20px; }
  `]  // ❌ NUNCA usar styles inline
})
export class UserListComponent {}
```

**Estructura de archivos por componente:**
```
user-list/
├── user-list.component.ts      # Lógica del componente
├── user-list.component.html    # Template HTML
└── user-list.component.css     # Estilos CSS
```

---

## 3. PATRONES OBLIGATORIOS

### 2.1 Standalone Components y Zoneless Development
**Todos los componentes deben ser standalone. Angular 20 promueve aplicaciones sin zone.js.**

```typescript
// ✅ CORRECTO - Componente standalone con archivos separados
@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  changeDetection: ChangeDetectionStrategy.OnPush, // SIEMPRE usar OnPush
  templateUrl: './user-list.component.html',      // SIEMPRE archivo separado
  styleUrl: './user-list.component.css'           // SIEMPRE archivo separado
})
export class UserListComponent {}

// ❌ INCORRECTO - No usar template/styles inline
@Component({
  selector: 'app-user-list',
  standalone: true,
  template: `<div>Inline template</div>`,  // ❌ PROHIBIDO
  styles: [`div { color: red; }`]           // ❌ PROHIBIDO
})

// ✅ CORRECTO - Configuración Zoneless en main.ts
bootstrapApplication(AppComponent, {
  providers: [
    provideExperimentalZonelessChangeDetection(), // Habilita desarrollo sin zone.js
    provideRouter(routes),
    provideHttpClient()
  ]
});

// ❌ INCORRECTO - No usar NgModule (considerado legacy)
@NgModule({
  declarations: [UserListComponent],
  imports: [CommonModule]
})
export class UserModule {}

// ❌ INCORRECTO - No omitir changeDetection OnPush
@Component({
  selector: 'app-user-list',
  standalone: true
  // falta changeDetection: ChangeDetectionStrategy.OnPush
})
```

### 2.2 Signals para Estado (Signal-First Architecture)
**Usar Signals como base para la reactividad. Evitar RxJS para estado interno.**

```typescript
// ✅ CORRECTO - Arquitectura Signal-first
export class UserListComponent {
  users = signal<User[]>([]);
  loading = signal<boolean>(false);
  error = signal<string>('');
  
  // Computed signals para valores derivados
  activeUsers = computed(() => this.users().filter(u => u.isActive));
  totalUsers = computed(() => this.users().length);
  
  // effect() para reaccionar a cambios
  constructor() {
    effect(() => {
      console.log('Users changed:', this.users().length);
    });
  }
}

// ✅ TAMBIÉN CORRECTO - LinkedSignal para estados dependientes pero reseteables
export class FilterComponent {
  searchTerm = signal('');
  // linkedSignal permite resetear manualmente aunque dependa de searchTerm
  filteredResults = linkedSignal(() => this.computeResults(this.searchTerm()));
  
  resetFilters() {
    this.filteredResults.set([]);
  }
}

// ❌ INCORRECTO - No usar BehaviorSubject para estado interno
export class UserListComponent {
  private usersSubject = new BehaviorSubject<User[]>([]);
  users$ = this.usersSubject.asObservable();
}

// ❌ INCORRECTO - No usar propiedades tradicionales para estado reactivo
export class UserListComponent {
  users: User[] = [];
  loading: boolean = false;
}
```

### 2.3 Servicios con RxJS
**Los servicios usan HttpClient con Observables.**

```typescript
// ✅ CORRECTO
@Injectable({ providedIn: 'root' })
export class UserService {
  private apiUrl = `${environment.apiUrl}/users`;
  private http = inject(HttpClient);

  getAll(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }

  getById(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  create(user: CreateUserDto): Observable<User> {
    return this.http.post<User>(this.apiUrl, user);
  }

  update(id: number, user: UpdateUserDto): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/${id}`, user);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

// ❌ INCORRECTO - No usar Promises o async/await en servicios HTTP
async getAll(): Promise<User[]> {
  return await fetch(this.apiUrl).then(r => r.json());
}
```

### 2.4 Reactive Forms
**Usar FormBuilder para formularios con validaciones.**

```typescript
// ✅ CORRECTO
export class UserFormComponent {
  private fb = inject(FormBuilder);
  
  userForm = this.fb.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    phoneNumber: ['', [Validators.pattern(/^\+?[1-9]\d{1,14}$/)]]
  });

  onSubmit(): void {
    if (this.userForm.valid) {
      const formValue = this.userForm.value;
      // Procesar formulario
    }
  }
}

// ❌ INCORRECTO - No usar Template-driven forms
<form #f="ngForm" (ngSubmit)="onSubmit(f)">
  <input name="firstName" [(ngModel)]="user.firstName">
</form>
```

### 2.5 Interceptors Funcionales
**Usar HttpInterceptorFn para interceptors.**

```typescript
// ✅ CORRECTO
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('auth_token');
  
  if (token) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }
  
  return next(req);
};

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An error occurred';
      
      if (error.error instanceof ErrorEvent) {
        // Client-side error
        errorMessage = error.error.message;
      } else {
        // Server-side error
        errorMessage = error.error?.message || error.message;
      }
      
      console.error('HTTP Error:', errorMessage);
      return throwError(() => new Error(errorMessage));
    })
  );
};

// ❌ INCORRECTO - No usar class-based interceptors
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // ...
  }
}
```

### 2.6 Guards Funcionales
**Usar CanActivateFn para guards.**

```typescript
// ✅ CORRECTO
export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLoggedIn()) {
    return true;
  }

  router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
  return false;
};

export const roleGuard = (allowedRoles: string[]): CanActivateFn => {
  return (route, state) => {
    const authService = inject(AuthService);
    const router = inject(Router);
    
    const userRole = authService.getUserRole();
    
    if (allowedRoles.includes(userRole)) {
      return true;
    }
    
    router.navigate(['/unauthorized']);
    return false;
  };
};

// ❌ INCORRECTO - No usar class-based guards
@Injectable()
export class AuthGuard implements CanActivate {
  canActivate(): boolean {
    // ...
  }
}
```

### 2.7 Lazy Loading de Rutas
**Cargar componentes de forma lazy.**

```typescript
// ✅ CORRECTO
export const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'users',
    loadComponent: () => import('./features/users/components/user-list/user-list.component')
      .then(m => m.UserListComponent),
    canActivate: [authGuard]
  },
  {
    path: 'users/:id',
    loadComponent: () => import('./features/users/components/user-detail/user-detail.component')
      .then(m => m.UserDetailComponent)
  },
  {
    path: 'users/:id/edit',
    loadComponent: () => import('./features/users/components/user-form/user-form.component')
      .then(m => m.UserFormComponent)
  }
];

// ❌ INCORRECTO - No cargar todos los componentes de forma eager
import { UserListComponent } from './features/users/components/user-list/user-list.component';

export const routes: Routes = [
  { path: 'users', component: UserListComponent }
];
```

### 2.8 Inyección de Dependencias Funcional e Input/Output Signals
**Usar inject() y las nuevas APIs de input(), output() y model().**

```typescript
// ✅ CORRECTO - inject() con input/output signals
export class UserCardComponent {
  // Nueva API input() en lugar de @Input
  user = input.required<User>();
  showDetails = input<boolean>(false);
  
  // Nueva API output() en lugar de @Output
  userSelected = output<User>();
  userDeleted = output<number>();
  
  // Nueva API model() para two-way binding
  isExpanded = model<boolean>(false);
  
  // inject() para dependencias
  private userService = inject(UserService);
  private router = inject(Router);
  
  onSelect() {
    this.userSelected.emit(this.user());
  }
  
  onDelete() {
    this.userDeleted.emit(this.user().id);
  }
}

// Uso en template
@Component({
  templateUrl: './parent.component.html',
  // En producción SIEMPRE usar archivo separado
})
export class ParentComponent {
  currentUser = signal<User>({ id: 1, name: 'John' });
  cardExpanded = signal(false);
  
  handleSelection(user: User) {
    console.log('Selected:', user);
  }
  
  handleDeletion(id: number) {
    console.log('Deleted:', id);
  }
}

// parent.component.html
/**
<app-user-card 
  [user]="currentUser()" 
  [showDetails]="true"
  [(isExpanded)]="cardExpanded"
  (userSelected)="handleSelection($event)"
  (userDeleted)="handleDeletion($event)"
/>
*/

// ❌ INCORRECTO - Constructor injection (NO PERMITIDO)
export class UserListComponent {
  private users = signal<User[]>([]);
  
  constructor(
    private userService: UserService,
    private router: Router
  ) {}
}

// ❌ INCORRECTO - Decoradores antiguos @Input/@Output
export class UserCardComponent {
  @Input() user!: User;
  @Output() userSelected = new EventEmitter<User>();
}
```

### 2.9 Control Flow Syntax (Angular 17+) y @defer
**Usar @if, @for, @switch en lugar de directivas estructurales. Implementar @defer para optimización.**

**NOTA**: Los siguientes ejemplos muestran templates inline solo con fines didácticos. En código de producción, **SIEMPRE** usar archivos HTML separados con `templateUrl`.

```typescript
// ✅ CORRECTO - Nueva sintaxis (el HTML debe estar en archivo .html separado)
@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',  // SIEMPRE archivo separado
  styleUrl: './user-list.component.css'
})

// user-list.component.html
/**
@if (loading()) {
  <div class="spinner">Loading...</div>
} @else if (error()) {
  <div class="error">{{ error() }}</div>
} @else {
  <table>
    <tbody>
      @for (user of users(); track user.id) {
        <tr>
          <td>{{ user.firstName }}</td>
          <td>{{ user.email }}</td>
        </tr>
      }
    </tbody>
  </table>
}

@switch (userType()) {
  @case ('admin') {
    <div>Admin Panel</div>
  }
  @case ('user') {
    <div>User Dashboard</div>
  }
  @default {
    <div>Guest View</div>
  }
}

<!-- @defer para componentes pesados (mejora Core Web Vitals - LCP) -->
@defer (on viewport) {
  <app-heavy-chart [data]="chartData()"/>
} @placeholder {
  <div class="chart-placeholder">Chart will load when visible</div>
} @loading (minimum 1s) {
  <div class="spinner">Loading chart...</div>
} @error {
  <div class="error">Failed to load chart</div>
}

<!-- @defer con trigger de interacción -->
@defer (on interaction) {
  <app-user-comments [userId]="currentUser().id"/>
} @placeholder {
  <button>Click to load comments</button>
}
*/

// ❌ INCORRECTO - Sintaxis antigua (hasta 90% más lenta)
<div *ngIf="loading">Loading...</div>
<div *ngFor="let user of users">{{ user.name }}</div>
<ng-container *ngSwitch="userType">
  <div *ngSwitchCase="'admin'">Admin</div>
</ng-container>
```

### 2.10 Manejo de Subscripciones
**Usar async pipe o effect() para manejar subscripciones automáticamente.**

```typescript
// ✅ CORRECTO - Usando async pipe
@Component({
  template: `
    @if (users$ | async; as users) {
      @for (user of users; track user.id) {
        <div>{{ user.name }}</div>
      }
    }
  `
})
export class UserListComponent {
  private userService = inject(UserService);
  
  users$ = this.userService.getAll();
}

// ✅ CORRECTO - Usando effect() con Signals
export class UserListComponent {
  private userService = inject(UserService);
  
  users = signal<User[]>([]);
  
  constructor() {
    effect(() => {
      this.loadUsers();
    });
  }
  
  private loadUsers(): void {
    this.userService.getAll().subscribe({
      next: (users) => this.users.set(users)
    });
  }
}

// ❌ INCORRECTO - Subscripciones sin limpiar
export class UserListComponent implements OnInit {
  users: User[] = [];
  
  ngOnInit() {
    this.userService.getAll().subscribe(users => {
      this.users = users; // Memory leak!
    });
  }
}
```

---

## 3. RENDERIZADO Y RENDIMIENTO (SSR & HYDRATION)

### 3.1 Server-Side Rendering con Event Replay
**Habilitar hidratación completa con Event Replay para capturar interacciones antes de la carga de JS.**

```typescript
// ✅ CORRECTO - Configuración en main.ts con Event Replay
import { bootstrapApplication } from '@angular/platform-browser';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';

bootstrapApplication(AppComponent, {
  providers: [
    provideClientHydration(withEventReplay()), // Event Replay habilitado
    provideExperimentalZonelessChangeDetection(),
    provideRouter(routes),
    provideHttpClient()
  ]
});

// Configuración SSR en server.ts
import { APP_BASE_HREF } from '@angular/common';
import { CommonEngine } from '@angular/ssr';

// Las interacciones del usuario antes de la hidratación se capturan y reproducen
```

### 3.2 Partial Hydration
**Usar hidratación parcial para componentes estáticos.**

```typescript
// ✅ CORRECTO - Componentes estáticos con archivos separados
@Component({
  selector: 'app-static-footer',
  standalone: true,
  templateUrl: './static-footer.component.html',  // Archivo separado
  styleUrl: './static-footer.component.css'
  // En Angular 20, los componentes sin interactividad pueden ser parcialmente hidratados
  // Esto reduce el bundle JS considerablemente en aplicaciones grandes
})
export class StaticFooterComponent {}

// static-footer.component.html
/**
<footer>
  <p>© 2026 Company Name. All rights reserved.</p>
</footer>
*/

// Configuración de estrategias de hidratación
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {}

// app.component.html
/**
<!-- Componentes interactivos se hidratan completamente -->
<app-header />

<!-- Componentes estáticos pueden usar hidratación parcial -->
<app-static-footer />
*/
```

### 3.3 Optimización de Core Web Vitals
**Usar @defer estratégicamente para mejorar LCP (Largest Contentful Paint).**

```typescript
// ✅ CORRECTO - Componente con archivo HTML separado
@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css'
})
export class MainPageComponent {
  heroData = signal({...});
  articles = signal([...]);
  metrics = signal({...});
}

// main-page.component.html - Priorizar contenido crítico, diferir lo secundario
/**
<!-- Contenido crítico carga inmediatamente -->
<app-hero-section [data]="heroData()" />
<app-main-content [articles]="articles()" />

<!-- Componentes pesados se difieren hasta que sean necesarios -->
@defer (on viewport; prefetch on idle) {
  <app-analytics-dashboard [metrics]="metrics()" />
} @placeholder {
  <div class="dashboard-skeleton"></div>
}

<!-- Widgets interactivos se difieren hasta la interacción -->
@defer (on interaction) {
  <app-comments-widget [postId]="currentPost().id" />
} @placeholder {
  <button class="load-comments">Load Comments</button>
}
*/
```

---

## 4. CONVENCIONES DE NOMBRADO

| Tipo | Convención | Ejemplo |
|------|------------|---------|
| Componente | kebab-case.ts | `user-list.ts` (sin .component.ts*) |
| Servicio | kebab-case.ts | `user.ts` (sin .service.ts*) |
| Guard | kebab-case.guard.ts | `auth.guard.ts` |
| Interceptor | kebab-case.interceptor.ts | `error.interceptor.ts` |
| Pipe | kebab-case.pipe.ts | `truncate.pipe.ts` |
| Directive | kebab-case.directive.ts | `auto-focus.directive.ts` |
| Model | kebab-case.model.ts | `user.model.ts` |
| Interface | PascalCase | `User`, `CreateUserDto` |
| Enum | PascalCase | `UserRole`, `OrderStatus` |
| Constante | UPPER_SNAKE_CASE | `API_BASE_URL`, `MAX_RETRY_ATTEMPTS` |

**Nota**: La tendencia en Angular 20 es omitir sufijos `.component.ts` y `.service.ts` para simplificar la estructura. Sin embargo, esto es **opcional** y depende de la preferencia del equipo.

---

## 5. ESTRUCTURA DE ARCHIVOS POR FEATURE

```
features/
└── [feature-name]/
    ├── components/
    │   ├── [feature]-list/
    │   │   ├── [feature]-list.ts          # Opcional: sin .component.ts
    │   │   ├── [feature]-list.html
    │   │   └── [feature]-list.css
    │   ├── [feature]-form/
    │   │   ├── [feature]-form.ts
    │   │   ├── [feature]-form.html
    │   │   └── [feature]-form.css
    │   └── [feature]-detail/
    │       ├── [feature]-detail.ts
    │       ├── [feature]-detail.html
    │       └── [feature]-detail.css
    ├── services/
    │   └── [feature].ts                    # Opcional: sin .service.ts
    └── models/
        └── [feature].model.ts
```

---

## 6. REGLAS DE CÓDIGO

1. **NO usar módulos NgModule** - Solo standalone components (legacy code)
2. **SIEMPRE separar HTML, CSS y TS** - Usar templateUrl/styleUrl (NUNCA inline)
3. **USAR Signals como base de reactividad** - Evitar BehaviorSubject para estado interno
4. **USAR input(), output(), model()** - En lugar de @Input/@Output
5. **USAR inject() para TODAS las dependencias** - NO usar constructor injection
6. **HABILITAR Zoneless Development** - Con provideExperimentalZonelessChangeDetection()
7. **CONFIGURAR OnPush en TODOS los componentes** - ChangeDetectionStrategy.OnPush
8. **USAR TypeScript 5.5+ en modo strict** - Todas las flags estrictas activadas
9. **USAR Esbuild/Vite** - Ya configurado por defecto en Angular CLI
10. **NO instalar librerías de terceros sin autorización**
11. **SIEMPRE usar Reactive Forms** para formularios
12. **SIEMPRE usar lazy loading** para rutas de features
13. **SIEMPRE usar HttpClient con Observables** en servicios
14. **SIEMPRE usar async pipe o effect()** para subscripciones
15. **SIEMPRE usar nueva sintaxis @if/@for/@switch** (hasta 90% más rápida)
16. **SIEMPRE usar @defer** para componentes pesados (optimiza LCP)
17. **SIEMPRE usar track en @for** para optimizar rendering
18. **HABILITAR hidratación con Event Replay** - provideClientHydration(withEventReplay())
19. **CONSIDERAR Partial Hydration** - Para componentes estáticos en apps grandes
20. **TODO el código en inglés**
21. **NO usar CSS preprocessors** - Solo CSS puro

---

## 7. EJEMPLO COMPLETO DE FEATURE

```typescript
// models/product.model.ts
export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  stock: number;
  isActive: boolean;
  createdAt: string;
}

export interface CreateProductDto {
  name: string;
  description: string;
  price: number;
  stock: number;
}

export interface UpdateProductDto {
  name: string;
  description: string;
  price: number;
  stock: number;
}

// services/product.service.ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Product, CreateProductDto, UpdateProductDto } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private apiUrl = `${environment.apiUrl}/products`;
  private http = inject(HttpClient);

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }

  getById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`);
  }

  create(product: CreateProductDto): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, product);
  }

  update(id: number, product: UpdateProductDto): Observable<Product> {
    return this.http.put<Product>(`${this.apiUrl}/${id}`, product);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

// components/product-list/product-list.component.ts
import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  changeDetection: ChangeDetectionStrategy.OnPush, // OBLIGATORIO
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit {
  private productService = inject(ProductService);
  private router = inject(Router);

  products = signal<Product[]>([]);
  loading = signal<boolean>(false);
  error = signal<string>('');
  
  // Computed signals para valores derivados
  activeProducts = computed(() => 
    this.products().filter(p => p.isActive)
  );
  totalStock = computed(() => 
    this.products().reduce((sum, p) => sum + p.stock, 0)
  );

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading.set(true);
    this.error.set('');

    this.productService.getAll().subscribe({
      next: (products) => {
        this.products.set(products);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Error loading products');
        this.loading.set(false);
        console.error('Error:', err);
      }
    });
  }

  onEdit(id: number): void {
    this.router.navigate(['/products', id, 'edit']);
  }

  onDelete(id: number): void {
    if (confirm('Are you sure you want to delete this product?')) {
      this.productService.delete(id).subscribe({
        next: () => {
          this.products.update(products => 
            products.filter(p => p.id !== id)
          );
        },
        error: (err) => {
          this.error.set('Error deleting product');
          console.error('Error:', err);
        }
      });
    }
  }
}

// components/product-list/product-list.component.html
<div class="container">
  <div class="header">
    <h1>Products</h1>
    <button class="btn btn-primary" routerLink="/products/new">
      Add Product
    </button>
  </div>

  @if (loading()) {
    <div class="loading">
      <div class="spinner"></div>
      <p>Loading products...</p>
    </div>
  }

  @if (error()) {
    <div class="alert alert-error">
      {{ error() }}
      <button class="btn btn-sm" (click)="loadProducts()">Retry</button>
    </div>
  }

  @if (!loading() && !error()) {
    <!-- Usar @defer para componentes pesados -->
    @defer (on viewport; prefetch on idle) {
      <div class="stats-summary">
        <div class="stat">
          <span>Total Products:</span>
          <strong>{{ products().length }}</strong>
        </div>
        <div class="stat">
          <span>Active Products:</span>
          <strong>{{ activeProducts().length }}</strong>
        </div>
        <div class="stat">
          <span>Total Stock:</span>
          <strong>{{ totalStock() }}</strong>
        </div>
      </div>
    } @placeholder {
      <div class="stats-skeleton"></div>
    }
    
    <table class="table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Name</th>
          <th>Price</th>
          <th>Stock</th>
          <th>Status</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        @for (product of products(); track product.id) {
          <tr>
            <td>{{ product.id }}</td>
            <td>{{ product.name }}</td>
            <td>{{ product.price | currency }}</td>
            <td>{{ product.stock }}</td>
            <td>
              <span [class]="product.isActive ? 'badge-success' : 'badge-danger'">
                {{ product.isActive ? 'Active' : 'Inactive' }}
              </span>
            </td>
            <td>
              <button class="btn btn-sm btn-info" [routerLink]="['/products', product.id]">
                View
              </button>
              <button class="btn btn-sm btn-primary" (click)="onEdit(product.id)">
                Edit
              </button>
              <button class="btn btn-sm btn-danger" (click)="onDelete(product.id)">
                Delete
              </button>
            </td>
          </tr>
        } @empty {
          <tr>
            <td colspan="6" class="text-center">No products found</td>
          </tr>
        }
      </tbody>
    </table>
  }
</div>

// components/product-form/product-form.component.ts
import { Component, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { CreateProductDto, UpdateProductDto } from '../../models/product.model';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush, // OBLIGATORIO
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.css'
})
export class ProductFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private productService = inject(ProductService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  productId = signal<number | null>(null);
  loading = signal<boolean>(false);
  error = signal<string>('');

  productForm = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    description: ['', [Validators.required, Validators.maxLength(500)]],
    price: [0, [Validators.required, Validators.min(0)]],
    stock: [0, [Validators.required, Validators.min(0)]]
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.productId.set(+id);
      this.loadProduct(+id);
    }
  }

  loadProduct(id: number): void {
    this.loading.set(true);
    this.productService.getById(id).subscribe({
      next: (product) => {
        this.productForm.patchValue(product);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Error loading product');
        this.loading.set(false);
        console.error('Error:', err);
      }
    });
  }

  onSubmit(): void {
    if (this.productForm.invalid) {
      this.productForm.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.error.set('');

    const formValue = this.productForm.value;
    const productData = {
      name: formValue.name!,
      description: formValue.description!,
      price: formValue.price!,
      stock: formValue.stock!
    };

    const request$ = this.productId() 
      ? this.productService.update(this.productId()!, productData as UpdateProductDto)
      : this.productService.create(productData as CreateProductDto);

    request$.subscribe({
      next: () => {
        this.router.navigate(['/products']);
      },
      error: (err) => {
        this.error.set('Error saving product');
        this.loading.set(false);
        console.error('Error:', err);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/products']);
  }
}
```

---

## 8. TOOLING Y BUILD (ESBUILD/VITE)

### 8.1 Angular CLI con Esbuild
**Angular 20 usa Esbuild y Vite por defecto - compilaciones hasta 5-10x más rápidas.**

```json
// angular.json - Configuración automática
{
  "projects": {
    "app": {
      "architect": {
        "build": {
          "builder": "@angular-devkit/build-angular:application",
          "options": {
            "outputPath": "dist/app",
            "index": "src/index.html",
            "browser": "src/main.ts",
            "polyfills": ["zone.js"], // Remover para zoneless
            "tsConfig": "tsconfig.app.json"
          }
        }
      }
    }
  }
}
```

### 8.2 TypeScript 5.5+ Strict Mode
**Mantener todas las flags estrictas activadas.**

```json
// tsconfig.json
{
  "compilerOptions": {
    "target": "ES2022",
    "module": "ES2022",
    "lib": ["ES2022", "dom"],
    "strict": true,                      // OBLIGATORIO
    "strictNullChecks": true,            // OBLIGATORIO
    "strictFunctionTypes": true,         // OBLIGATORIO
    "strictPropertyInitialization": true, // OBLIGATORIO
    "noImplicitAny": true,               // OBLIGATORIO
    "noImplicitReturns": true,
    "noFallthroughCasesInSwitch": true,
    "noUnusedLocals": true,
    "noUnusedParameters": true,
    "skipLibCheck": true,
    "esModuleInterop": true,
    "forceConsistentCasingInFileNames": true
  }
}
```

---

## 9. CHECKLIST PARA NUEVO CÓDIGO

- [ ] Componente es standalone
- [ ] Archivos separados: .ts, .html, .css (NO inline template/styles)
- [ ] ChangeDetectionStrategy.OnPush configurado
- [ ] Usa Signals para estado reactivo (signal(), computed(), effect())
- [ ] Usa input(), output(), model() en lugar de @Input/@Output
- [ ] Usa inject() para TODAS las dependencias (NO constructor injection)
- [ ] Nueva sintaxis @if/@for/@switch (no *ngIf/*ngFor)
- [ ] @defer implementado para componentes pesados
- [ ] Reactive Forms para formularios
- [ ] Lazy loading configurado en rutas
- [ ] Guards funcionales aplicados
- [ ] Interceptors funcionales registrados
- [ ] Servicios con HttpClient y Observables
- [ ] Manejo de errores implementado
- [ ] Loading states con Signals
- [ ] Track function en @for loops
- [ ] TypeScript strict mode activado
- [ ] Hidratación con Event Replay habilitada
- [ ] Zoneless development considerado
- [ ] Código en inglés
- [ ] CSS puro sin preprocessors
- [ ] **Documentación actualizada** (ver [Feature Documentation Skill](../feature-documentation/SKILL.md))

---

## 10. DOCUMENTACIÓN DE FEATURES

**⚠️ OBLIGATORIO**: Al completar el desarrollo de cualquier feature o modificación, debes actualizar la documentación siguiendo el **[Feature Documentation Skill](../feature-documentation/SKILL.md)**.

### Cuándo Documentar (Frontend)

✅ **SIEMPRE documentar cuando:**
- Creas un nuevo servicio que consume API
- Modificas la interfaz de un servicio existente
- Cambias modelos/interfaces de datos
- Agregas nuevas validaciones en formularios
- Creas componentes reutilizables importantes
- Implementas nuevos guards o interceptors

### Documentación de Servicios Angular

Cuando creas/modificas un servicio que consume endpoints backend:

```typescript
// user.service.ts - Documentar en docs/features/USERS.md
@Injectable({ providedIn: 'root' })
export class UserService {
  // Este servicio debe estar documentado con:
  // - Métodos públicos con descripción
  // - Interfaces de request/response
  // - Manejo de errores
  // - Ejemplos de uso desde componentes
}
```

### Qué Documentar (Frontend)

1. **Servicios**: Métodos públicos con ejemplos
2. **Interfaces/Types**: Modelos de datos del frontend
3. **Guards**: Condiciones y comportamiento
4. **Interceptors**: Qué transforman/agregan
5. **Componentes Shared**: Props, events, uso

### Integración con Backend

- Los ejemplos de Angular deben ir en la misma documentación del feature backend
- Sección "Angular 20 Service Example" en cada feature
- Mantener sincronizados los modelos TypeScript con los DTOs de C#

### Proceso

```
Componente/Servicio Completo → Actualizar Docs → Commit (código + docs) → PR
```

**Referencia completa**: Ver [Feature Documentation Skill](../feature-documentation/SKILL.md)

---

## 11. TECNOLOGÍAS Y VERSIONES

- Angular 20
- TypeScript 5.5+
- RxJS 7.x (solo para servicios HTTP)
- CSS puro (sin preprocessors)
- Standalone Components (NgModule es legacy)
- Signals API (Signal-first architecture)
- Control Flow Syntax (@if, @for, @switch) - 90% más rápido
- Esbuild/Vite (build tool por defecto)
- Zoneless Development (experimental)
- SSR con Event Replay y Partial Hydration

---

**IMPORTANTE**: Cualquier desviación de estas reglas debe ser consultada y aprobada antes de implementarse.
