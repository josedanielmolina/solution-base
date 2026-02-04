import { Component, signal, ChangeDetectionStrategy, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CountryService } from '../../services/country.service';
import { Country } from '../../models/country.model';

@Component({
    selector: 'app-country-list-page',
    standalone: true,
    imports: [CommonModule, RouterLink],
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './country-list.page.html',
    styleUrl: './country-list.page.css'
})
export class CountryListPage implements OnInit {
    private countryService = inject(CountryService);

    countries = signal<Country[]>([]);
    loading = signal<boolean>(true);
    errorMessage = signal<string>('');

    ngOnInit(): void {
        this.loadCountries();
    }

    loadCountries(): void {
        this.loading.set(true);
        this.countryService.getAll(false).subscribe({
            next: (countries) => {
                this.countries.set(countries);
                this.loading.set(false);
            },
            error: (error) => {
                this.loading.set(false);
                this.errorMessage.set(error.error?.message || 'Error al cargar países');
            }
        });
    }

    deleteCountry(country: Country): void {
        if (confirm(`¿Desactivar el país "${country.name}"?`)) {
            this.countryService.delete(country.id).subscribe({
                next: () => this.loadCountries(),
                error: (error) => {
                    this.errorMessage.set(error.error?.message || 'Error al eliminar');
                }
            });
        }
    }
}
