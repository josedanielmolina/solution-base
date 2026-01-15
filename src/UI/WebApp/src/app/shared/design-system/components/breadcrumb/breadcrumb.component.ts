import { Component, computed, input, output } from '@angular/core';

export interface BreadcrumbItem {
  label: string;
  href?: string;
  icon?: string;
}

@Component({
  selector: 'ds-breadcrumb',
  standalone: true,
  templateUrl: './breadcrumb.component.html',
  styleUrl: './breadcrumb.component.css',
  host: {
    'class': 'ds-breadcrumb-wrapper'
  }
})
export class BreadcrumbComponent {
  // Inputs
  readonly items = input.required<BreadcrumbItem[]>();
  readonly separator = input<'slash' | 'chevron' | 'arrow'>('chevron');
  readonly size = input<'sm' | 'md' | 'lg'>('md');

  // Outputs
  readonly navigate = output<BreadcrumbItem>();

  // Computed
  readonly breadcrumbClasses = computed(() => {
    return ['ds-breadcrumb', `ds-breadcrumb--${this.size()}`].join(' ');
  });

  readonly separatorIcon = computed(() => {
    const icons = {
      slash: 'M9 5l-4 14',
      chevron: 'M9 18l6-6-6-6',
      arrow: 'M5 12h14M12 5l7 7-7 7'
    };
    return icons[this.separator()];
  });

  onNavigate(item: BreadcrumbItem): void {
    this.navigate.emit(item);
  }
}
