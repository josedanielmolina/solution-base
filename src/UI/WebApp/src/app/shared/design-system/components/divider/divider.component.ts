import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'ds-divider',
  standalone: true,
  templateUrl: './divider.component.html',
  styleUrl: './divider.component.css',
  host: {
    'class': 'ds-divider-wrapper',
    'role': 'separator'
  }
})
export class DividerComponent {
  // Inputs
  readonly orientation = input<'horizontal' | 'vertical'>('horizontal');
  readonly variant = input<'solid' | 'dashed' | 'dotted'>('solid');
  readonly label = input<string | undefined>(undefined);
  readonly labelPosition = input<'start' | 'center' | 'end'>('center');
  readonly spacing = input<'sm' | 'md' | 'lg'>('md');

  // Computed
  readonly dividerClasses = computed(() => {
    const classes = [
      'ds-divider',
      `ds-divider--${this.orientation()}`,
      `ds-divider--${this.variant()}`,
      `ds-divider--spacing-${this.spacing()}`
    ];
    if (this.label()) {
      classes.push('ds-divider--with-label');
      classes.push(`ds-divider--label-${this.labelPosition()}`);
    }
    return classes.join(' ');
  });
}
