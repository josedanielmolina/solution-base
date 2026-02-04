# Skill: Refinamiento de Documentos Funcionales - App Torneos Padel

## Descripción
Este skill tiene como objetivo brindas las guias necesarias para el desarrollo de la aplicación.

**Reglas Obligatorias Angular:**
1. Standalone + OnPush + Archivos separados (.ts/.html/.css) SIEMPRE
2. Signals (NO BehaviorSubject) + input()/output()/model() (NO @Input/@Output)
3. inject() SIEMPRE (NO constructor injection)
4. @if/@for/@switch (NO *ngIf/*ngFor) + @defer + track en @for
5. Reactive Forms + Lazy loading + Guards/Interceptors funcionales
6. Encapsular librerías de terceros en servicios
7. Tailwind CSS
8. TypeScript strict + Código en inglés

**Reglas Obligatorias .NET:**
1. Result Pattern SIEMPRE (NO excepciones para flujo) | Repository + UoW
2. 1 Feature = 1 caso de uso, en su archivo
3. FluentValidation en DTOs | Controllers solo delegan
4. Entidades: constructores privados + factory methods
5. Entity Configurations separadas 
6. Código en inglés