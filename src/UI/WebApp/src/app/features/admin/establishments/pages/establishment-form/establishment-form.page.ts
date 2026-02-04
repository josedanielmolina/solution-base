import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { EstablishmentService } from '../../services/establishment.service';
import { CountryService } from '../../../countries/services/country.service';
import { CityService } from '../../../cities/services/city.service';
import { SCHEDULE_TYPES } from '../../models/establishment.model';
import { Country } from '../../../countries/models/country.model';
import { City } from '../../../cities/models/city.model';

@Component({
    selector: 'app-establishment-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, RouterLink],
    templateUrl: './establishment-form.page.html',
    styleUrl: './establishment-form.page.css'
})
export class EstablishmentFormPage implements OnInit {
    private readonly fb = inject(FormBuilder);
    private readonly service = inject(EstablishmentService);
    private readonly countryService = inject(CountryService);
    private readonly cityService = inject(CityService);
    private readonly route = inject(ActivatedRoute);
    private readonly router = inject(Router);

    form!: FormGroup;
    isEdit = signal(false);
    loading = signal(false);
    saving = signal(false);
    error = signal<string | null>(null);
    establishmentId: number | null = null;

    countries = signal<Country[]>([]);
    cities = signal<City[]>([]);
    scheduleTypes = SCHEDULE_TYPES;

    ngOnInit(): void {
        this.initForm();
        this.loadCountries();

        const id = this.route.snapshot.params['id'];
        if (id && id !== 'new') {
            this.establishmentId = +id;
            this.isEdit.set(true);
            this.loadEstablishment();
        }
    }

    private initForm(): void {
        this.form = this.fb.group({
            name: ['', [Validators.required, Validators.maxLength(100)]],
            countryId: ['', Validators.required],
            cityId: ['', Validators.required],
            address: ['', [Validators.required, Validators.maxLength(500)]],
            googleMapsUrl: ['', Validators.maxLength(500)],
            phoneLandline: ['', Validators.maxLength(20)],
            phoneMobile: ['', Validators.maxLength(20)],
            logo: [''],
            scheduleType: [1, Validators.required],
            isActive: [true]
        });

        // Listen for country changes to reload cities
        this.form.get('countryId')?.valueChanges.subscribe(countryId => {
            if (countryId) {
                this.loadCities(+countryId);
                if (!this.isEdit()) {
                    this.form.patchValue({ cityId: '' });
                }
            }
        });
    }

    private loadCountries(): void {
        this.countryService.getAll().subscribe({
            next: (data) => this.countries.set(data),
            error: (err) => console.error('Error loading countries', err)
        });
    }

    private loadCities(countryId: number): void {
        this.cityService.getByCountry(countryId).subscribe({
            next: (data) => this.cities.set(data),
            error: (err) => console.error('Error loading cities', err)
        });
    }

    private loadEstablishment(): void {
        if (!this.establishmentId) return;

        this.loading.set(true);
        this.service.getById(this.establishmentId).subscribe({
            next: (data) => {
                // Load cities first
                this.loadCities(data.countryId);

                this.form.patchValue({
                    name: data.name,
                    countryId: data.countryId,
                    cityId: data.cityId,
                    address: data.address,
                    googleMapsUrl: data.googleMapsUrl || '',
                    phoneLandline: data.phoneLandline || '',
                    phoneMobile: data.phoneMobile || '',
                    logo: data.logo || '',
                    scheduleType: data.scheduleType,
                    isActive: data.isActive
                });
                this.loading.set(false);
            },
            error: (err) => {
                this.error.set('Error al cargar el establecimiento');
                this.loading.set(false);
                console.error(err);
            }
        });
    }

    onLogoChange(event: Event): void {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files[0]) {
            const file = input.files[0];

            // Check file size (max 5MB)
            if (file.size > 5 * 1024 * 1024) {
                this.error.set('La imagen no puede exceder 5MB');
                return;
            }

            const reader = new FileReader();
            reader.onload = () => {
                this.form.patchValue({ logo: reader.result as string });
            };
            reader.readAsDataURL(file);
        }
    }

    removeLogo(): void {
        this.form.patchValue({ logo: '' });
    }

    onSubmit(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        // Validate at least one phone
        const { phoneLandline, phoneMobile } = this.form.value;
        if (!phoneLandline && !phoneMobile) {
            this.error.set('Debe proporcionar al menos un teléfono');
            return;
        }

        this.saving.set(true);
        this.error.set(null);

        const formValue = this.form.value;
        const data = {
            ...formValue,
            countryId: parseInt(formValue.countryId, 10),
            cityId: parseInt(formValue.cityId, 10),
            scheduleType: parseInt(formValue.scheduleType, 10)
        };

        const operation = this.isEdit()
            ? this.service.update(this.establishmentId!, data)
            : this.service.create(data);

        operation.subscribe({
            next: (result) => {
                this.saving.set(false);
                // Navigate to detail if create, or list if edit
                if (this.isEdit()) {
                    this.router.navigate(['/app/admin/establishments']);
                } else {
                    this.router.navigate(['/app/admin/establishments', result.id]);
                }
            },
            error: (err) => {
                this.saving.set(false);
                this.error.set(err.error?.message || 'Error al guardar');
                console.error(err);
            }
        });
    }
}
