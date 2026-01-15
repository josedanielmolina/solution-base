import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'ds-badge',
  standalone: true,
  templateUrl: './badge.component.html',
  styleUrl: './badge.component.css',
  host: {
    'class': 'ds-badge-wrapper'
  }
})
export class BadgeComponent {
  // Inputs
  readonly variant = input<'primary' | 'secondary' | 'success' | 'warning' | 'danger' | 'info'>('primary');
  readonly size = input<'sm' | 'md' | 'lg'>('md');
  readonly rounded = input(false);
  readonly dot = input(false);
  readonly outline = input(false);

  // Computed
  readonly badgeClasses = computed(() => {
    const classes = [
      'ds-badge',
      `ds-badge--${this.variant()}`,
      `ds-badge--${this.size()}`
    ];
    if (this.rounded()) classes.push('ds-badge--rounded');
    if (this.dot()) classes.push('ds-badge--dot');
    if (this.outline()) classes.push('ds-badge--outline');
    return classes.join(' ');
  });
}
