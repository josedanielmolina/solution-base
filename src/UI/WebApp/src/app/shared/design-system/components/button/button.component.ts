import { Component, computed, input, output } from '@angular/core';

export type ButtonVariant = 'primary' | 'secondary' | 'success' | 'ghost' | 'dark' | 'danger' | 'warning' | 'link';
export type ButtonSize = 'xs' | 'sm' | 'md' | 'lg' | 'xl';

@Component({
  selector: 'ds-button',
  standalone: true,
  templateUrl: './button.component.html',
  styleUrl: './button.component.css',
  host: {
    'class': 'ds-button-wrapper'
  }
})
export class ButtonComponent {
  // Inputs using signal-based API
  readonly variant = input<ButtonVariant>('primary');
  readonly size = input<ButtonSize>('md');
  readonly disabled = input(false);
  readonly loading = input(false);
  readonly fullWidth = input(false);
  readonly type = input<'button' | 'submit' | 'reset'>('button');
  readonly ariaLabel = input<string | undefined>(undefined);

  // Outputs using signal-based API
  readonly clicked = output<MouseEvent>();

  // Computed property for CSS classes
  readonly buttonClasses = computed(() => {
    const classes = [
      'ds-button',
      `ds-button--${this.variant()}`,
      `ds-button--${this.size()}`
    ];

    if (this.fullWidth()) classes.push('ds-button--full-width');
    if (this.loading()) classes.push('ds-button--loading');
    if (this.disabled()) classes.push('ds-button--disabled');

    return classes.join(' ');
  });

  // Computed for disabled state
  readonly isDisabled = computed(() => this.disabled() || this.loading());

  onClick(event: MouseEvent): void {
    if (!this.isDisabled()) {
      this.clicked.emit(event);
    }
  }
}
