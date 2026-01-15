import { Component, computed, input, output } from '@angular/core';

@Component({
  selector: 'ds-empty-state',
  standalone: true,
  templateUrl: './empty-state.component.html',
  styleUrl: './empty-state.component.css',
  host: {
    'class': 'ds-empty-state-wrapper'
  }
})
export class EmptyStateComponent {
  // Inputs
  readonly title = input.required<string>();
  readonly description = input<string | undefined>(undefined);
  readonly icon = input<'search' | 'document' | 'folder' | 'inbox' | 'error' | 'custom'>('inbox');
  readonly size = input<'sm' | 'md' | 'lg'>('md');
  readonly actionLabel = input<string | undefined>(undefined);

  // Outputs
  readonly action = output<void>();

  // Computed
  readonly containerClasses = computed(() => {
    return ['ds-empty-state', `ds-empty-state--${this.size()}`].join(' ');
  });

  readonly iconPath = computed(() => {
    const icons: Record<string, string> = {
      search: 'M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z',
      document: 'M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z',
      folder: 'M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z',
      inbox: 'M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4',
      error: 'M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z'
    };
    return icons[this.icon()] || icons['inbox'];
  });

  onAction(): void {
    this.action.emit();
  }
}
