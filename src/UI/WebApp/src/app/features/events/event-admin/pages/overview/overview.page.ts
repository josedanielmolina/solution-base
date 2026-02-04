import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { EventService } from '../../../services/event.service';
import { EventDto } from '../../../models/event.model';

@Component({
    selector: 'app-event-overview',
    standalone: true,
    imports: [CommonModule, RouterLink],
    templateUrl: './overview.page.html',
    styleUrl: './overview.page.css'
})
export class EventOverviewPage implements OnInit {
    private readonly route = inject(ActivatedRoute);
    private readonly service = inject(EventService);

    event = signal<EventDto | null>(null);
    loading = signal(true);

    ngOnInit(): void {
        // Get event from parent route
        const publicId = this.route.parent?.snapshot.paramMap.get('id');
        if (publicId) {
            this.loadEvent(publicId);
        }
    }

    loadEvent(publicId: string): void {
        this.service.getEvent(publicId).subscribe({
            next: (event) => {
                this.event.set(event);
                this.loading.set(false);
            },
            error: () => this.loading.set(false)
        });
    }
}
