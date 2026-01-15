import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'ds-spinner',
  standalone: true,
  templateUrl: './spinner.component.html',
  styleUrl: './spinner.component.css',
  host: {
    'class': 'ds-spinner-wrapper',
    'role': 'status',
    '[attr.aria-label]': 'ariaLabel()'
  }
})
export class SpinnerComponent {
  // Inputs
  readonly size = input<'xs' | 'sm' | 'md' | 'lg' | 'xl'>('md');
  readonly variant = input<'primary' | 'secondary' | 'white'>('primary');
  readonly ariaLabel = input('Cargando');

  // Computed
  readonly spinnerClasses = computed(() => {
    return ['ds-spinner', `ds-spinner--${this.size()}`, `ds-spinner--${this.variant()}`].join(' ');
  });
}
