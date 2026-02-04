import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { EventService } from '../../../services/event.service';
import { EventEstablishmentDto } from '../../../models/event.model';

@Component({
    selector: 'app-event-establishments',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './establishments.page.html',
    styleUrl: './establishments.page.css'
})
export class EventEstablishmentsPage implements OnInit {
    private readonly route = inject(ActivatedRoute);
    private readonly service = inject(EventService);

    establishments = signal<EventEstablishmentDto[]>([]);
    availableEstablishments = signal<any[]>([]);
    loading = signal(true);
    adding = signal(false);
    error = signal<string | null>(null);
    publicId = signal<string>('');
    searchQuery = signal('');
    showSearch = signal(false);

    ngOnInit(): void {
        const id = this.route.parent?.snapshot.paramMap.get('id');
        if (id) {
            this.publicId.set(id);
            this.loadEstablishments();
        }
    }

    loadEstablishments(): void {
        this.loading.set(true);
        this.service.getEstablishments(this.publicId()).subscribe({
            next: (data) => {
                this.establishments.set(data);
                this.loading.set(false);
            },
            error: () => {
                this.error.set('Error al cargar establecimientos');
                this.loading.set(false);
            }
        });
    }

    toggleSearch(): void {
        this.showSearch.update(v => !v);
        if (this.showSearch()) {
            this.searchAvailable();
        }
    }

    searchAvailable(): void {
        this.service.searchAvailableEstablishments(this.publicId(), this.searchQuery()).subscribe({
            next: (data) => this.availableEstablishments.set(data),
            error: () => this.availableEstablishments.set([])
        });
    }

    addEstablishment(establishmentId: number): void {
        this.adding.set(true);
        this.service.addEstablishment(this.publicId(), { establishmentId }).subscribe({
            next: () => {
                this.loadEstablishments();
                this.searchAvailable();
                this.adding.set(false);
            },
            error: (err) => {
                this.error.set(err.error?.message || 'Error al agregar establecimiento');
                this.adding.set(false);
            }
        });
    }

    removeEstablishment(establishment: EventEstablishmentDto): void {
        if (confirm(`¿Eliminar "${establishment.establishmentName}" del evento?`)) {
            this.service.removeEstablishment(this.publicId(), establishment.establishmentId).subscribe({
                next: () => this.loadEstablishments(),
                error: () => this.error.set('Error al eliminar establecimiento')
            });
        }
    }
}
