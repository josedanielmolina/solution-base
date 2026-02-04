import { Component, signal, ChangeDetectionStrategy, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CategoryService } from '../../services/category.service';
import { CountryService } from '@features/admin/countries/services/country.service';
import { Country } from '@features/admin/countries/models/country.model';
import { GENDERS } from '../../models/category.model';

@Component({
    selector: 'app-category-form-page',
    standalone: true,
    imports: [CommonModule, RouterLink, ReactiveFormsModule],
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './category-form.page.html',
    styleUrl: './category-form.page.css'
})
export class CategoryFormPage implements OnInit {
    private fb = inject(FormBuilder);
    private categoryService = inject(CategoryService);
    private countryService = inject(CountryService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);

    form: FormGroup;
    isEditMode = signal<boolean>(false);
    categoryId = signal<number | null>(null);
    countries = signal<Country[]>([]);
    genders = GENDERS;
    loading = signal<boolean>(false);
    saving = signal<boolean>(false);
    errorMessage = signal<string>('');

    constructor() {
        this.form = this.fb.group({
            name: ['', [Validators.required, Validators.maxLength(100)]],
            gender: ['', [Validators.required]],
            countryId: ['', [Validators.required]],
            isActive: [true]
        });
    }

    ngOnInit(): void {
        this.loadCountries();
        const id = this.route.snapshot.paramMap.get('id');
        if (id && id !== 'new') {
            this.isEditMode.set(true);
            this.categoryId.set(parseInt(id, 10));
            this.loadCategory(this.categoryId()!);
        }
    }

    loadCountries(): void {
        this.countryService.getAll(true).subscribe({
            next: (countries) => this.countries.set(countries),
            error: () => { }
        });
    }

    loadCategory(id: number): void {
        this.loading.set(true);
        this.categoryService.getById(id).subscribe({
            next: (category) => {
                const genderValue = this.genders.find(g =>
                    g.label.toLowerCase().includes(category.gender.toLowerCase()) ||
                    category.gender.toLowerCase().includes(g.label.toLowerCase().substring(0, 3))
                )?.value || 1;
                this.form.patchValue({
                    name: category.name,
                    gender: genderValue,
                    countryId: category.countryId,
                    isActive: category.isActive
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

        // Convert string values from form selects to numbers
        const payload = {
            name: data.name,
            gender: parseInt(data.gender, 10),
            countryId: parseInt(data.countryId, 10),
            isActive: data.isActive
        };

        const request = this.isEditMode()
            ? this.categoryService.update(this.categoryId()!, payload)
            : this.categoryService.create({ name: payload.name, gender: payload.gender, countryId: payload.countryId });

        request.subscribe({
            next: () => {
                this.saving.set(false);
                this.router.navigate(['/app/admin/categories']);
            },
            error: (error) => {
                this.saving.set(false);
                this.errorMessage.set(error.error?.message || 'Error al guardar');
            }
        });
    }
}
