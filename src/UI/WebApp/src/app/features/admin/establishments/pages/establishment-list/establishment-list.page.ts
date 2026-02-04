import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { EstablishmentService } from '../../services/establishment.service';
import { EstablishmentList } from '../../models/establishment.model';

@Component({
    selector: 'app-establishment-list',
    standalone: true,
    imports: [CommonModule, RouterLink],
    templateUrl: './establishment-list.page.html',
    styleUrl: './establishment-list.page.css'
})
export class EstablishmentListPage implements OnInit {
    private readonly service = inject(EstablishmentService);
    private readonly router = inject(Router);

    establishments = signal<EstablishmentList[]>([]);
    loading = signal(true);
    error = signal<string | null>(null);
    showInactive = signal(false);

    ngOnInit(): void {
        this.loadEstablishments();
    }

    loadEstablishments(): void {
        this.loading.set(true);
        this.error.set(null);

        this.service.getAll(!this.showInactive()).subscribe({
            next: (data) => {
                this.establishments.set(data);
                this.loading.set(false);
            },
            error: (err) => {
                this.error.set('Error al cargar establecimientos');
                this.loading.set(false);
                console.error(err);
            }
        });
    }

    toggleShowInactive(): void {
        this.showInactive.update(v => !v);
        this.loadEstablishments();
    }

    onEdit(id: number): void {
        this.router.navigate(['/app/admin/establishments', id, 'edit']);
    }

    onView(id: number): void {
        this.router.navigate(['/app/admin/establishments', id]);
    }

    onDelete(establishment: EstablishmentList): void {
        if (confirm(`¿Está seguro de eliminar "${establishment.name}"?`)) {
            this.service.delete(establishment.id).subscribe({
                next: () => this.loadEstablishments(),
                error: (err) => {
                    this.error.set('Error al eliminar el establecimiento');
                    console.error(err);
                }
            });
        }
    }
}
