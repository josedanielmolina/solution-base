import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'ds-tooltip',
  standalone: true,
  templateUrl: './tooltip.component.html',
  styleUrl: './tooltip.component.css',
  host: {
    'class': 'ds-tooltip-wrapper'
  }
})
export class TooltipComponent {
  // Inputs
  readonly content = input.required<string>();
  readonly position = input<'top' | 'bottom' | 'left' | 'right'>('top');
  readonly variant = input<'dark' | 'light'>('dark');
  readonly showArrow = input(true);
  readonly delay = input(300);

  // Computed
  readonly tooltipClasses = computed(() => {
    const classes = [
      'ds-tooltip',
      `ds-tooltip--${this.position()}`,
      `ds-tooltip--${this.variant()}`
    ];
    return classes.join(' ');
  });

  readonly containerStyle = computed(() => {
    return { '--tooltip-delay': `${this.delay()}ms` };
  });
}
