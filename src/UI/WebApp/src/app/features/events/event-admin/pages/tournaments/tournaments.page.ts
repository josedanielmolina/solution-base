import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-event-tournaments',
    standalone: true,
    imports: [CommonModule],
    template: `
        <div class="page">
            <h2>Torneos</h2>
            <div class="coming-soon">
                <div class="icon">🏆</div>
                <h3>Próximamente</h3>
                <p>La gestión de torneos estará disponible en la Fase 5</p>
            </div>
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
            padding: 4rem 2rem;
            background: white;
            border-radius: 12px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
        }
        .coming-soon .icon {
            font-size: 4rem;
            margin-bottom: 1rem;
        }
        .coming-soon h3 {
            margin: 0 0 0.5rem;
            color: var(--text-primary, #1e293b);
        }
        .coming-soon p {
            margin: 0;
            color: var(--text-secondary, #64748b);
        }
    `]
})
export class EventTournamentsPage { }
