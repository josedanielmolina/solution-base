import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'ds-progress',
  standalone: true,
  templateUrl: './progress.component.html',
  styleUrl: './progress.component.css',
  host: {
    'class': 'ds-progress-wrapper',
    'role': 'progressbar',
    '[attr.aria-valuenow]': 'value()',
    '[attr.aria-valuemin]': '0',
    '[attr.aria-valuemax]': 'max()',
    '[attr.aria-label]': 'ariaLabel()'
  }
})
export class ProgressComponent {
  // Inputs
  readonly value = input(0);
  readonly max = input(100);
  readonly size = input<'sm' | 'md' | 'lg'>('md');
  readonly variant = input<'primary' | 'success' | 'warning' | 'danger' | 'info'>('primary');
  readonly showLabel = input(false);
  readonly striped = input(false);
  readonly animated = input(false);
  readonly indeterminate = input(false);
  readonly ariaLabel = input('Progreso');

  // Computed
  readonly percentage = computed(() => {
    const v = this.value();
    const m = this.max();
    return Math.min(Math.max((v / m) * 100, 0), 100);
  });

  readonly trackClasses = computed(() => {
    const classes = ['ds-progress-track', `ds-progress-track--${this.size()}`];
    if (this.indeterminate()) classes.push('ds-progress--indeterminate');
    return classes.join(' ');
  });

  readonly barClasses = computed(() => {
    const classes = ['ds-progress-bar'];
    // Map variant to CSS class
    if (this.variant() === 'primary') classes.push('ds-progress--primary');
    else if (this.variant() === 'success') classes.push('ds-progress--success');
    else if (this.variant() === 'warning') classes.push('ds-progress--warning');
    else if (this.variant() === 'danger') classes.push('ds-progress--danger');
    else if (this.variant() === 'info') classes.push('ds-progress--info');

    if (this.striped()) classes.push('ds-progress-bar--striped');
    if (this.animated()) classes.push('ds-progress-bar--animated');
    return classes.join(' ');
  });

  readonly barStyle = computed(() => {
    if (this.indeterminate()) return {};
    return { width: `${this.percentage()}%` };
  });

  readonly labelText = computed(() => {
    return `${Math.round(this.percentage())}%`;
  });
}
