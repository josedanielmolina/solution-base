import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'ds-skeleton',
  standalone: true,
  templateUrl: './skeleton.component.html',
  styleUrl: './skeleton.component.css',
  host: {
    'class': 'ds-skeleton-wrapper',
    'role': 'status',
    '[attr.aria-label]': 'ariaLabel()',
    '[attr.aria-busy]': 'true'
  }
})
export class SkeletonComponent {
  // Inputs
  readonly variant = input<'text' | 'circular' | 'rectangular' | 'rounded'>('text');
  readonly width = input<string | undefined>(undefined);
  readonly height = input<string | undefined>(undefined);
  readonly lines = input(1);
  readonly animated = input(true);
  readonly ariaLabel = input('Cargando contenido');

  // Computed
  readonly skeletonClasses = computed(() => {
    const classes = ['ds-skeleton', `ds-skeleton--${this.variant()}`];
    if (this.animated()) classes.push('ds-skeleton--animated');
    return classes.join(' ');
  });

  readonly skeletonStyle = computed(() => {
    const style: Record<string, string> = {};
    if (this.width()) style['width'] = this.width()!;
    if (this.height()) style['height'] = this.height()!;
    return style;
  });

  readonly lineArray = computed(() => {
    return Array(this.lines()).fill(0).map((_, i) => i);
  });
}
