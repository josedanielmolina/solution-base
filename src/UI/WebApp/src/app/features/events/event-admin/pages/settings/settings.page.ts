import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-event-settings',
    standalone: true,
    imports: [CommonModule],
    template: `
        <div class="page">
            <h2>Configuración</h2>
            <p class="coming-soon">Próximamente: Configuración del evento, posters, PDF de reglas...</p>
        </div>
    `,
    styles: [`
        .page h2 {
            margin: 0 0 1rem;
            font-size: 1.5rem;
            color: var(--text-primary, #1e293b);
        }
        .coming-soon {
            text-align: center;
            padding: 3rem;
            background: white;
            border-radius: 12px;
            color: var(--text-secondary, #64748b);
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
        }
    `]
})
export class EventSettingsPage { }
