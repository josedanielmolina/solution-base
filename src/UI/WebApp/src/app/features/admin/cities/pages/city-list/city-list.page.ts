import { Component, signal, ChangeDetectionStrategy, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CityService } from '../../services/city.service';
import { City } from '../../models/city.model';

@Component({
    selector: 'app-city-list-page',
    standalone: true,
    imports: [CommonModule, RouterLink],
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './city-list.page.html',
    styleUrl: './city-list.page.css'
})
export class CityListPage implements OnInit {
    private cityService = inject(CityService);

    cities = signal<City[]>([]);
    loading = signal<boolean>(true);
    errorMessage = signal<string>('');

    ngOnInit(): void {
        this.loadCities();
    }

    loadCities(): void {
        this.loading.set(true);
        this.cityService.getAll(false).subscribe({
            next: (cities) => {
                this.cities.set(cities);
                this.loading.set(false);
            },
            error: (error) => {
                this.loading.set(false);
                this.errorMessage.set(error.error?.message || 'Error al cargar ciudades');
            }
        });
    }

    deleteCity(city: City): void {
        if (confirm(`¿Desactivar la ciudad "${city.name}"?`)) {
            this.cityService.delete(city.id).subscribe({
                next: () => this.loadCities(),
                error: (error) => {
                    this.errorMessage.set(error.error?.message || 'Error al eliminar');
                }
            });
        }
    }
}
