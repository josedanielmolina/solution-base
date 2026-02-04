import { Component, inject, OnInit, signal, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Subject, debounceTime, distinctUntilChanged, switchMap, of, takeUntil } from 'rxjs';
import { EventService } from '../../services/event.service';
import { CreateEventDto, UpdateEventDto } from '../../models/event.model';
import { UserService, UserSearchResult } from '../../../users/services/user.service';

@Component({
    selector: 'app-event-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, RouterLink],
    templateUrl: './event-form.page.html',
    styleUrl: './event-form.page.css'
})
export class EventFormPage implements OnInit, OnDestroy {
    private readonly fb = inject(FormBuilder);
    private readonly eventService = inject(EventService);
    private readonly userService = inject(UserService);
    private readonly route = inject(ActivatedRoute);
    private readonly router = inject(Router);

    private destroy$ = new Subject<void>();
    private searchSubject$ = new Subject<string>();

    form: FormGroup;
    isEditing = signal(false);
    loading = signal(false);
    submitting = signal(false);
    error = signal<string | null>(null);
    publicId = signal<string | null>(null);

    // Organizer autocomplete
    organizerSearch = signal('');
    organizerResults = signal<UserSearchResult[]>([]);
    selectedOrganizer = signal<UserSearchResult | null>(null);
    showOrganizerDropdown = signal(false);
    searchingOrganizers = signal(false);

    constructor() {
        this.form = this.fb.group({
            name: ['', [Validators.required, Validators.maxLength(200)]],
            description: [''],
            organizerId: [null], // Now optional
            contactPhone: ['', [Validators.maxLength(20)]],
            startDate: ['', [Validators.required]],
            endDate: ['', [Validators.required]],
            whatsApp: ['', [Validators.maxLength(50)]],
            facebook: ['', [Validators.maxLength(200)]],
            instagram: ['', [Validators.maxLength(200)]]
        });
    }

    ngOnInit(): void {
        // Setup debounced search
        this.searchSubject$
            .pipe(
                debounceTime(300),
                distinctUntilChanged(),
                switchMap(query => {
                    if (!query || query.length < 2) {
                        return of([]);
                    }
                    this.searchingOrganizers.set(true);
                    return this.userService.search(query, 5);
                }),
                takeUntil(this.destroy$)
            )
            .subscribe({
                next: (results) => {
                    this.organizerResults.set(results);
                    this.searchingOrganizers.set(false);
                    this.showOrganizerDropdown.set(results.length > 0);
                },
                error: () => {
                    this.organizerResults.set([]);
                    this.searchingOrganizers.set(false);
                }
            });

        const id = this.route.snapshot.paramMap.get('id');
        if (id && id !== 'new') {
            this.publicId.set(id);
            this.isEditing.set(true);
            this.loadEvent(id);
        }
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    loadEvent(publicId: string): void {
        this.loading.set(true);
        this.eventService.getEvent(publicId).subscribe({
            next: (event) => {
                this.form.patchValue({
                    name: event.name,
                    description: event.description,
                    organizerId: event.organizerId,
                    contactPhone: event.contactPhone,
                    startDate: this.formatDateForInput(event.startDate),
                    endDate: this.formatDateForInput(event.endDate),
                    whatsApp: event.whatsApp,
                    facebook: event.facebook,
                    instagram: event.instagram
                });
                // Set selected organizer for display
                if (event.organizerId && event.organizerName) {
                    this.selectedOrganizer.set({
                        id: event.organizerId,
                        firstName: event.organizerName.split(' ')[0] || '',
                        lastName: event.organizerName.split(' ').slice(1).join(' ') || '',
                        email: '',
                        document: null
                    });
                    this.organizerSearch.set(event.organizerName);
                }
                this.loading.set(false);
            },
            error: (err) => {
                this.error.set('Error al cargar el evento');
                this.loading.set(false);
                console.error(err);
            }
        });
    }

    onOrganizerSearch(event: Event): void {
        const value = (event.target as HTMLInputElement).value;
        this.organizerSearch.set(value);
        this.searchSubject$.next(value);

        // If user clears the input, clear the selection
        if (!value) {
            this.selectedOrganizer.set(null);
            this.form.patchValue({ organizerId: null });
        }
    }

    selectOrganizer(user: UserSearchResult): void {
        this.selectedOrganizer.set(user);
        this.organizerSearch.set(`${user.firstName} ${user.lastName}`);
        this.form.patchValue({ organizerId: user.id });
        this.showOrganizerDropdown.set(false);
        this.organizerResults.set([]);
    }

    clearOrganizer(): void {
        this.selectedOrganizer.set(null);
        this.organizerSearch.set('');
        this.form.patchValue({ organizerId: null });
    }

    hideDropdown(): void {
        // Delay to allow click on dropdown item
        setTimeout(() => {
            this.showOrganizerDropdown.set(false);
        }, 200);
    }

    onSubmit(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        this.submitting.set(true);
        this.error.set(null);

        const formValue = this.form.value;

        if (this.isEditing()) {
            const updateDto: UpdateEventDto = {
                name: formValue.name,
                description: formValue.description,
                organizerId: formValue.organizerId,
                contactPhone: formValue.contactPhone,
                startDate: new Date(formValue.startDate),
                endDate: new Date(formValue.endDate),
                whatsApp: formValue.whatsApp,
                facebook: formValue.facebook,
                instagram: formValue.instagram
            };

            this.eventService.updateEvent(this.publicId()!, updateDto).subscribe({
                next: () => {
                    this.router.navigate(['/app/events']);
                },
                error: (err) => {
                    this.error.set(err.error?.message || 'Error al actualizar el evento');
                    this.submitting.set(false);
                }
            });
        } else {
            const createDto: CreateEventDto = {
                name: formValue.name,
                description: formValue.description,
                organizerId: formValue.organizerId, // Can be null
                contactPhone: formValue.contactPhone,
                startDate: new Date(formValue.startDate),
                endDate: new Date(formValue.endDate),
                whatsApp: formValue.whatsApp,
                facebook: formValue.facebook,
                instagram: formValue.instagram
            };

            this.eventService.createEvent(createDto).subscribe({
                next: (created) => {
                    this.router.navigate(['/app/events', created.publicId]);
                },
                error: (err) => {
                    this.error.set(err.error?.message || 'Error al crear el evento');
                    this.submitting.set(false);
                }
            });
        }
    }

    private formatDateForInput(date: Date): string {
        const d = new Date(date);
        return d.toISOString().split('T')[0];
    }

    hasError(field: string): boolean {
        const control = this.form.get(field);
        return control ? control.invalid && control.touched : false;
    }
}
