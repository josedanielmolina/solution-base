import { Component, computed, input, output, signal } from '@angular/core';

@Component({
  selector: 'ds-pagination',
  standalone: true,
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css',
  host: {
    'class': 'ds-pagination-wrapper'
  }
})
export class PaginationComponent {
  // Inputs
  readonly totalItems = input.required<number>();
  readonly itemsPerPage = input(10);
  readonly currentPage = input(1);
  readonly maxVisiblePages = input(5);
  readonly size = input<'sm' | 'md' | 'lg'>('md');
  readonly showFirstLast = input(true);
  readonly showPrevNext = input(true);

  // Internal state
  private readonly internalPage = signal<number | null>(null);

  // Outputs
  readonly pageChange = output<number>();

  // Computed
  readonly activePage = computed(() => {
    return this.internalPage() ?? this.currentPage();
  });

  readonly totalPages = computed(() => {
    return Math.ceil(this.totalItems() / this.itemsPerPage());
  });

  readonly visiblePages = computed(() => {
    const total = this.totalPages();
    const current = this.activePage();
    const max = this.maxVisiblePages();

    if (total <= max) {
      return Array.from({ length: total }, (_, i) => i + 1);
    }

    const half = Math.floor(max / 2);
    let start = Math.max(current - half, 1);
    let end = start + max - 1;

    if (end > total) {
      end = total;
      start = Math.max(end - max + 1, 1);
    }

    const pages: (number | string)[] = [];

    if (start > 1) {
      pages.push(1);
      if (start > 2) pages.push('...');
    }

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }

    if (end < total) {
      if (end < total - 1) pages.push('...');
      pages.push(total);
    }

    return pages;
  });

  readonly canGoPrevious = computed(() => this.activePage() > 1);
  readonly canGoNext = computed(() => this.activePage() < this.totalPages());

  readonly paginationClasses = computed(() => {
    return ['ds-pagination', `ds-pagination--${this.size()}`].join(' ');
  });

  goToPage(page: number | string): void {
    if (typeof page === 'string') return;
    if (page < 1 || page > this.totalPages()) return;

    this.internalPage.set(page);
    this.pageChange.emit(page);
  }

  goToFirst(): void {
    this.goToPage(1);
  }

  goToLast(): void {
    this.goToPage(this.totalPages());
  }

  goToPrevious(): void {
    if (this.canGoPrevious()) {
      this.goToPage(this.activePage() - 1);
    }
  }

  goToNext(): void {
    if (this.canGoNext()) {
      this.goToPage(this.activePage() + 1);
    }
  }

  isCurrentPage(page: number | string): boolean {
    return page === this.activePage();
  }
}
