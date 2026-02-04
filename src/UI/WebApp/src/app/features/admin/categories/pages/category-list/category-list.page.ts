import { Component, signal, ChangeDetectionStrategy, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CategoryService } from '../../services/category.service';
import { Category, GENDERS } from '../../models/category.model';

@Component({
    selector: 'app-category-list-page',
    standalone: true,
    imports: [CommonModule, RouterLink],
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './category-list.page.html',
    styleUrl: './category-list.page.css'
})
export class CategoryListPage implements OnInit {
    private categoryService = inject(CategoryService);

    categories = signal<Category[]>([]);
    loading = signal<boolean>(true);
    errorMessage = signal<string>('');
    genders = GENDERS;

    ngOnInit(): void {
        this.loadCategories();
    }

    getGenderLabel(gender: string): string {
        const found = this.genders.find(g => g.label.toLowerCase().includes(gender.toLowerCase()) || gender.toLowerCase().includes(g.label.toLowerCase()));
        return found?.label || gender;
    }

    loadCategories(): void {
        this.loading.set(true);
        this.categoryService.getAll(false).subscribe({
            next: (categories) => {
                this.categories.set(categories);
                this.loading.set(false);
            },
            error: (error) => {
                this.loading.set(false);
                this.errorMessage.set(error.error?.message || 'Error al cargar categorías');
            }
        });
    }

    deleteCategory(category: Category): void {
        if (confirm(`¿Desactivar la categoría "${category.name}"?`)) {
            this.categoryService.delete(category.id).subscribe({
                next: () => this.loadCategories(),
                error: (error) => {
                    this.errorMessage.set(error.error?.message || 'Error al eliminar');
                }
            });
        }
    }
}
