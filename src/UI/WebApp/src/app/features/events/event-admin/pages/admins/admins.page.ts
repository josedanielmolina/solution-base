import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { EventService } from '../../../services/event.service';
import { EventAdminDto, EventInvitationDto } from '../../../models/event.model';

@Component({
    selector: 'app-event-admins',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './admins.page.html',
    styleUrl: './admins.page.css'
})
export class EventAdminsPage implements OnInit {
    private readonly route = inject(ActivatedRoute);
    private readonly service = inject(EventService);

    admins = signal<EventAdminDto[]>([]);
    pendingInvitations = signal<EventInvitationDto[]>([]);
    loading = signal(true);
    error = signal<string | null>(null);
    publicId = signal<string>('');

    // Invite modal
    showInviteModal = signal(false);
    inviteEmail = signal('');
    inviting = signal(false);

    ngOnInit(): void {
        const id = this.route.parent?.snapshot.paramMap.get('id');
        if (id) {
            this.publicId.set(id);
            this.loadData();
        }
    }

    loadData(): void {
        this.loading.set(true);
        this.service.getAdmins(this.publicId()).subscribe({
            next: (admins) => {
                this.admins.set(admins);
                this.loadInvitations();
            },
            error: () => {
                this.error.set('Error al cargar administradores');
                this.loading.set(false);
            }
        });
    }

    loadInvitations(): void {
        this.service.getPendingInvitations(this.publicId()).subscribe({
            next: (invitations) => {
                this.pendingInvitations.set(invitations);
                this.loading.set(false);
            },
            error: () => this.loading.set(false)
        });
    }

    openInviteModal(): void {
        this.showInviteModal.set(true);
        this.inviteEmail.set('');
    }

    closeInviteModal(): void {
        this.showInviteModal.set(false);
    }

    sendInvitation(): void {
        const email = this.inviteEmail().trim();
        if (!email) return;

        this.inviting.set(true);
        this.service.inviteAdmin(this.publicId(), { email }).subscribe({
            next: () => {
                this.closeInviteModal();
                this.loadData();
                this.inviting.set(false);
            },
            error: (err) => {
                this.error.set(err.error?.message || 'Error al enviar invitación');
                this.inviting.set(false);
            }
        });
    }

    removeAdmin(admin: EventAdminDto): void {
        if (confirm(`¿Eliminar a "${admin.userName}" como administrador?`)) {
            this.service.removeAdmin(this.publicId(), admin.userId).subscribe({
                next: () => this.loadData(),
                error: () => this.error.set('Error al eliminar administrador')
            });
        }
    }
}
