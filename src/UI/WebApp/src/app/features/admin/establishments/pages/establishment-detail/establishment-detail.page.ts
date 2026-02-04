import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { EstablishmentService } from '../../services/establishment.service';
import { Establishment, Court, Photo, COURT_TYPES, SCHEDULE_TYPES } from '../../models/establishment.model';
import { CourtFormComponent } from '../../components/court-form/court-form.component';
import { ScheduleFormComponent } from '../../components/schedule-form/schedule-form.component';

@Component({
    selector: 'app-establishment-detail',
    standalone: true,
    imports: [CommonModule, RouterLink, CourtFormComponent, ScheduleFormComponent],
    templateUrl: './establishment-detail.page.html',
    styleUrl: './establishment-detail.page.css'
})
export class EstablishmentDetailPage implements OnInit {
    private readonly service = inject(EstablishmentService);
    private readonly route = inject(ActivatedRoute);
    private readonly router = inject(Router);

    establishment = signal<Establishment | null>(null);
    loading = signal(true);
    error = signal<string | null>(null);

    showCourtForm = signal(false);
    editingCourt = signal<Court | null>(null);
    showScheduleForm = signal(false);
    showCourtPhotos = signal<Court | null>(null);

    courtTypes = COURT_TYPES;
    scheduleTypes = SCHEDULE_TYPES;

    ngOnInit(): void {
        const id = this.route.snapshot.params['id'];
        if (id) {
            this.loadEstablishment(+id);
        }
    }

    loadEstablishment(id: number): void {
        this.loading.set(true);
        this.service.getById(id).subscribe({
            next: (data) => {
                this.establishment.set(data);
                this.loading.set(false);
            },
            error: (err) => {
                this.error.set('Error al cargar el establecimiento');
                this.loading.set(false);
                console.error(err);
            }
        });
    }

    getScheduleTypeName(type: number): string {
        return this.scheduleTypes.find(t => t.value === type)?.label || 'N/A';
    }

    getCourtTypeName(type: number): string {
        return this.courtTypes.find(t => t.value === type)?.label || 'N/A';
    }

    // === Court Management ===
    openCourtForm(court?: Court): void {
        this.editingCourt.set(court || null);
        this.showCourtForm.set(true);
    }

    closeCourtForm(): void {
        this.showCourtForm.set(false);
        this.editingCourt.set(null);
    }

    onCourtSaved(): void {
        this.closeCourtForm();
        if (this.establishment()) {
            this.loadEstablishment(this.establishment()!.id);
        }
    }

    deleteCourt(court: Court): void {
        if (!confirm(`¿Está seguro de eliminar la cancha "${court.name}"?`)) return;

        const est = this.establishment();
        if (!est) return;

        this.service.deleteCourt(est.id, court.id).subscribe({
            next: () => this.loadEstablishment(est.id),
            error: (err) => {
                this.error.set('Error al eliminar la cancha');
                console.error(err);
            }
        });
    }

    // === Schedule Management ===
    openScheduleForm(): void {
        this.showScheduleForm.set(true);
    }

    closeScheduleForm(): void {
        this.showScheduleForm.set(false);
    }

    onSchedulesSaved(): void {
        this.closeScheduleForm();
        if (this.establishment()) {
            this.loadEstablishment(this.establishment()!.id);
        }
    }

    // === Photo Management (Establishment) ===
    onPhotoUpload(event: Event): void {
        const input = event.target as HTMLInputElement;
        const est = this.establishment();
        if (!input.files || !input.files[0] || !est) return;

        const file = input.files[0];
        if (file.size > 5 * 1024 * 1024) {
            this.error.set('La imagen no puede exceder 5MB');
            return;
        }

        const reader = new FileReader();
        reader.onload = () => {
            const displayOrder = est.photos.length;
            this.service.addPhoto(est.id, {
                imageData: reader.result as string,
                displayOrder
            }).subscribe({
                next: () => this.loadEstablishment(est.id),
                error: (err) => {
                    this.error.set('Error al subir la foto');
                    console.error(err);
                }
            });
        };
        reader.readAsDataURL(file);

        input.value = '';
    }

    removePhoto(photoId: number): void {
        if (!confirm('¿Está seguro de eliminar esta foto?')) return;

        const est = this.establishment();
        if (!est) return;

        this.service.removePhoto(est.id, photoId).subscribe({
            next: () => this.loadEstablishment(est.id),
            error: (err) => {
                this.error.set('Error al eliminar la foto');
                console.error(err);
            }
        });
    }

    // === Court Photos Management ===
    openCourtPhotos(court: Court): void {
        this.showCourtPhotos.set(court);
    }

    closeCourtPhotos(): void {
        this.showCourtPhotos.set(null);
    }

    onCourtPhotoUpload(event: Event): void {
        const input = event.target as HTMLInputElement;
        const court = this.showCourtPhotos();
        const est = this.establishment();
        if (!input.files || !input.files[0] || !court || !est) return;

        const file = input.files[0];
        if (file.size > 5 * 1024 * 1024) {
            this.error.set('La imagen no puede exceder 5MB');
            return;
        }

        const reader = new FileReader();
        reader.onload = () => {
            const displayOrder = court.photos.length;
            this.service.addCourtPhoto(est.id, court.id, {
                imageData: reader.result as string,
                displayOrder
            }).subscribe({
                next: () => {
                    this.loadEstablishment(est.id);
                    // Refresh court photos reference
                    setTimeout(() => {
                        const updatedCourt = this.establishment()?.courts.find(c => c.id === court.id);
                        if (updatedCourt) this.showCourtPhotos.set(updatedCourt);
                    }, 100);
                },
                error: (err) => {
                    this.error.set('Error al subir la foto de la cancha');
                    console.error(err);
                }
            });
        };
        reader.readAsDataURL(file);

        input.value = '';
    }

    removeCourtPhoto(photoId: number): void {
        if (!confirm('¿Está seguro de eliminar esta foto?')) return;

        const court = this.showCourtPhotos();
        const est = this.establishment();
        if (!court || !est) return;

        this.service.removeCourtPhoto(est.id, court.id, photoId).subscribe({
            next: () => {
                this.loadEstablishment(est.id);
                // Refresh court photos reference
                setTimeout(() => {
                    const updatedCourt = this.establishment()?.courts.find(c => c.id === court.id);
                    if (updatedCourt) this.showCourtPhotos.set(updatedCourt);
                }, 100);
            },
            error: (err) => {
                this.error.set('Error al eliminar la foto de la cancha');
                console.error(err);
            }
        });
    }
}
