import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterOutlet, ActivatedRoute } from '@angular/router';
import { EventService } from '../../services/event.service';
import { EventDto } from '../../models/event.model';

@Component({
    selector: 'app-event-layout',
    standalone: true,
    imports: [CommonModule, RouterLink, RouterOutlet],
    templateUrl: './event-layout.component.html',
    styleUrl: './event-layout.component.css'
})
export class EventLayoutComponent implements OnInit {
    private readonly route = inject(ActivatedRoute);
    private readonly router = inject(Router);
    private readonly service = inject(EventService);

    event = signal<EventDto | null>(null);
    loading = signal(true);
    error = signal<string | null>(null);
    publicId = signal<string>('');

    navItems = [
        { path: '', label: 'Resumen', icon: '📊', exact: true },
        { path: 'establishments', label: 'Establecimientos', icon: '🏟️', exact: false },
        { path: 'admins', label: 'Administradores', icon: '👥', exact: false },
        { path: 'tournaments', label: 'Torneos', icon: '🏆', exact: false },
        { path: 'settings', label: 'Configuración', icon: '⚙️', exact: false }
    ];

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (id) {
            this.publicId.set(id);
            this.loadEvent(id);
        }
    }

    loadEvent(publicId: string): void {
        this.loading.set(true);
        this.service.getEvent(publicId).subscribe({
            next: (event) => {
                this.event.set(event);
                this.loading.set(false);
            },
            error: (err) => {
                if (err.status === 403) {
                    this.error.set('No tiene acceso a este evento');
                } else if (err.status === 404) {
                    this.error.set('Evento no encontrado');
                } else {
                    this.error.set('Error al cargar el evento');
                }
                this.loading.set(false);
            }
        });
    }

    isActive(path: string, exact: boolean): boolean {
        const currentUrl = this.router.url;
        const basePath = `/app/events/${this.publicId()}`;
        const fullPath = path ? `${basePath}/${path}` : basePath;

        if (exact) {
            return currentUrl === fullPath;
        }
        return currentUrl.startsWith(fullPath);
    }
}
