import { Component, signal, ChangeDetectionStrategy, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CountryService } from '../../services/country.service';

@Component({
    selector: 'app-country-form-page',
    standalone: true,
    imports: [CommonModule, RouterLink, ReactiveFormsModule],
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './country-form.page.html',
    styleUrl: './country-form.page.css'
})
export class CountryFormPage implements OnInit {
    private fb = inject(FormBuilder);
    private countryService = inject(CountryService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);

    form: FormGroup;
    isEditMode = signal<boolean>(false);
    countryId = signal<number | null>(null);
    loading = signal<boolean>(false);
    saving = signal<boolean>(false);
    errorMessage = signal<string>('');

    constructor() {
        this.form = this.fb.group({
            name: ['', [Validators.required, Validators.maxLength(100)]],
            code: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(3), Validators.pattern(/^[A-Z]+$/)]],
            isActive: [true]
        });
    }

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');
        if (id && id !== 'new') {
            this.isEditMode.set(true);
            this.countryId.set(parseInt(id, 10));
            this.loadCountry(this.countryId()!);
        }
    }

    loadCountry(id: number): void {
        this.loading.set(true);
        this.countryService.getById(id).subscribe({
            next: (country) => {
                this.form.patchValue({
                    name: country.name,
                    code: country.code,
                    isActive: country.isActive
                });
                this.loading.set(false);
            },
            error: (error) => {
                this.loading.set(false);
                this.errorMessage.set(error.error?.message || 'Error al cargar el país');
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

        const request = this.isEditMode()
            ? this.countryService.update(this.countryId()!, data)
            : this.countryService.create({ name: data.name, code: data.code });

        request.subscribe({
            next: () => {
                this.saving.set(false);
                this.router.navigate(['/app/admin/countries']);
            },
            error: (error) => {
                this.saving.set(false);
                this.errorMessage.set(error.error?.message || 'Error al guardar');
            }
        });
    }
}
