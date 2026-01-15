import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'ds-card',
  standalone: true,
  templateUrl: './card.component.html',
  styleUrl: './card.component.css',
  host: {
    'class': 'ds-card-wrapper'
  }
})
export class CardComponent {
  // Inputs
  readonly variant = input<'elevated' | 'outlined' | 'filled'>('elevated');
  readonly padding = input<'none' | 'sm' | 'md' | 'lg'>('md');
  readonly interactive = input(false);
  readonly selected = input(false);

  // Computed
  readonly cardClasses = computed(() => {
    const classes = [
      'ds-card',
      `ds-card--${this.variant()}`,
      `ds-card--padding-${this.padding()}`
    ];
    if (this.interactive()) classes.push('ds-card--interactive');
    if (this.selected()) classes.push('ds-card--selected');
    return classes.join(' ');
  });
}
