import { Component, computed, input, output } from '@angular/core';

@Component({
  selector: 'ds-alert',
  standalone: true,
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.css',
  host: {
    'class': 'ds-alert-wrapper',
    'role': 'alert'
  }
})
export class AlertComponent {
  // Inputs
  readonly variant = input<'success' | 'warning' | 'danger' | 'info'>('info');
  readonly title = input<string | undefined>(undefined);
  readonly dismissible = input(false);
  readonly icon = input(true);
  readonly bordered = input(true);
  readonly filled = input(false);

  // Outputs
  readonly dismiss = output<void>();

  // Computed
  readonly alertClasses = computed(() => {
    const classes = ['ds-alert', `ds-alert--${this.variant()}`];
    if (this.bordered()) classes.push('ds-alert--bordered');
    if (this.filled()) classes.push('ds-alert--filled');
    return classes.join(' ');
  });

  readonly iconPath = computed(() => {
    const icons: Record<string, string> = {
      success: 'M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z',
      warning: 'M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z',
      danger: 'M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z',
      info: 'M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z'
    };
    return icons[this.variant()];
  });

  onDismiss(): void {
    this.dismiss.emit();
  }
}
