import { Directive, Input, TemplateRef, ViewContainerRef, inject, effect, signal } from '@angular/core';
import { AuthService } from '@core/services/auth.service';

@Directive({
    selector: '[hasPermission]',
    standalone: true
})
export class HasPermissionDirective {
    private templateRef = inject(TemplateRef<any>);
    private viewContainer = inject(ViewContainerRef);
    private authService = inject(AuthService);

    private hasView = false;
    private permission = signal<string | string[]>('');
    private requireAll = signal<boolean>(false);

    @Input()
    set hasPermission(value: string | string[]) {
        this.permission.set(value);
        this.updateView();
    }

    @Input()
    set hasPermissionRequireAll(value: boolean) {
        this.requireAll.set(value);
        this.updateView();
    }

    constructor() {
        // React to user changes
        effect(() => {
            const user = this.authService.currentUser();
            if (user) {
                this.updateView();
            }
        });
    }

    private updateView(): void {
        const permission = this.permission();
        const requireAll = this.requireAll();

        let hasAccess = false;

        if (typeof permission === 'string') {
            hasAccess = this.authService.hasPermission(permission);
        } else if (Array.isArray(permission)) {
            hasAccess = requireAll
                ? this.authService.hasAllPermissions(permission)
                : this.authService.hasAnyPermission(permission);
        }

        if (hasAccess && !this.hasView) {
            this.viewContainer.createEmbeddedView(this.templateRef);
            this.hasView = true;
        } else if (!hasAccess && this.hasView) {
            this.viewContainer.clear();
            this.hasView = false;
        }
    }
}
