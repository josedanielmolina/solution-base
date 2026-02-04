import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { EventService } from '../../services/event.service';
import { EventListDto } from '../../models/event.model';

@Component({
    selector: 'app-event-list',
    standalone: true,
    imports: [CommonModule, RouterLink],
    templateUrl: './event-list.page.html',
    styleUrl: './event-list.page.css'
})
export class EventListPage implements OnInit {
    private readonly service = inject(EventService);
    private readonly router = inject(Router);

    events = signal<EventListDto[]>([]);
    loading = signal(true);
    error = signal<string | null>(null);

    ngOnInit(): void {
        this.loadEvents();
    }

    loadEvents(): void {
        this.loading.set(true);
        this.error.set(null);

        this.service.getMyEvents().subscribe({
            next: (data) => {
                this.events.set(data);
                this.loading.set(false);
            },
            error: (err) => {
                this.error.set('Error al cargar eventos');
                this.loading.set(false);
                console.error(err);
            }
        });
    }

    onManage(publicId: string): void {
        this.router.navigate(['/app/events', publicId]);
    }

    onDelete(event: EventListDto): void {
        if (confirm(`¿Está seguro de eliminar el evento "${event.name}"?`)) {
            this.service.deleteEvent(event.publicId).subscribe({
                next: () => this.loadEvents(),
                error: (err) => {
                    this.error.set('Error al eliminar el evento');
                    console.error(err);
                }
            });
        }
    }

    formatDate(date: Date): string {
        return new Date(date).toLocaleDateString('es-ES', {
            day: '2-digit',
            month: 'short',
            year: 'numeric'
        });
    }
}
