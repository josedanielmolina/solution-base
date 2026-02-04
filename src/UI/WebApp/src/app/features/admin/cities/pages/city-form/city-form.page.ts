import { Component, signal, ChangeDetectionStrategy, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CityService } from '../../services/city.service';
import { CountryService } from '@features/admin/countries/services/country.service';
import { Country } from '@features/admin/countries/models/country.model';

@Component({
    selector: 'app-city-form-page',
    standalone: true,
    imports: [CommonModule, RouterLink, ReactiveFormsModule],
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './city-form.page.html',
    styleUrl: './city-form.page.css'
})
export class CityFormPage implements OnInit {
    private fb = inject(FormBuilder);
    private cityService = inject(CityService);
    private countryService = inject(CountryService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);

    form: FormGroup;
    isEditMode = signal<boolean>(false);
    cityId = signal<number | null>(null);
    countries = signal<Country[]>([]);
    loading = signal<boolean>(false);
    saving = signal<boolean>(false);
    errorMessage = signal<string>('');

    constructor() {
        this.form = this.fb.group({
            name: ['', [Validators.required, Validators.maxLength(100)]],
            countryId: ['', [Validators.required]],
            isActive: [true]
        });
    }

    ngOnInit(): void {
        this.loadCountries();
        const id = this.route.snapshot.paramMap.get('id');
        if (id && id !== 'new') {
            this.isEditMode.set(true);
            this.cityId.set(parseInt(id, 10));
            this.loadCity(this.cityId()!);
        }
    }

    loadCountries(): void {
        this.countryService.getAll(true).subscribe({
            next: (countries) => this.countries.set(countries),
            error: () => { }
        });
    }

    loadCity(id: number): void {
        this.loading.set(true);
        this.cityService.getById(id).subscribe({
            next: (city) => {
                this.form.patchValue({
                    name: city.name,
                    countryId: city.countryId,
                    isActive: city.isActive
                });
                this.loading.set(false);
            },
            error: (error) => {
                this.loading.set(false);
                this.errorMessage.set(error.error?.message || 'Error al cargar');
            }
        });
    }

    submit(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        this.saving.set(true);
        this.errorMessage.set('');
        const data = this.form.value;

        // Convert string value from form select to number
        const payload = {
            name: data.name,
            countryId: parseInt(data.countryId, 10),
            isActive: data.isActive
        };

        const request = this.isEditMode()
            ? this.cityService.update(this.cityId()!, payload)
            : this.cityService.create({ name: payload.name, countryId: payload.countryId });

        request.subscribe({
            next: () => {
                this.saving.set(false);
                this.router.navigate(['/app/admin/cities']);
            },
            error: (error) => {
                this.saving.set(false);
                this.errorMessage.set(error.error?.message || 'Error al guardar');
            }
        });
    }
}
