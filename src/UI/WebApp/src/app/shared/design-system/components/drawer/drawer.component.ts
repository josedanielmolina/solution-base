import { Component, computed, input, output, signal, HostListener, OnDestroy } from '@angular/core';

@Component({
  selector: 'ds-drawer',
  standalone: true,
  templateUrl: './drawer.component.html',
  styleUrl: './drawer.component.css',
  host: {
    'class': 'ds-drawer-host'
  }
})
export class DrawerComponent implements OnDestroy {
  // Inputs
  readonly isOpen = input(false);
  readonly title = input<string | undefined>(undefined);
  readonly position = input<'left' | 'right' | 'top' | 'bottom'>('right');
  readonly size = input<'sm' | 'md' | 'lg' | 'xl' | 'full'>('md');
  readonly closeOnBackdrop = input(true);
  readonly closeOnEscape = input(true);
  readonly showClose = input(true);
  readonly ariaLabel = input<string | undefined>(undefined);

  // Outputs
  readonly close = output<void>();

  // Internal state
  readonly isVisible = signal(false);

  // Computed
  readonly drawerClasses = computed(() => {
    const classes = [
      'ds-drawer',
      `ds-drawer--${this.position()}`,
      `ds-drawer--${this.size()}`
    ];
    if (this.isOpen()) classes.push('ds-drawer--open');
    return classes.join(' ');
  });

  readonly overlayClasses = computed(() => {
    const classes = ['ds-drawer-overlay'];
    if (this.isOpen()) classes.push('ds-drawer-overlay--visible');
    return classes.join(' ');
  });

  ngOnDestroy(): void {
    this.enableBodyScroll();
  }

  closeDrawer(): void {
    this.enableBodyScroll();
    this.close.emit();
  }

  onBackdropClick(event: MouseEvent): void {
    if (this.closeOnBackdrop() && event.target === event.currentTarget) {
      this.closeDrawer();
    }
  }

  @HostListener('document:keydown.escape')
  onEscapeKey(): void {
    if (this.closeOnEscape() && this.isOpen()) {
      this.closeDrawer();
    }
  }

  private disableBodyScroll(): void {
    document.body.style.overflow = 'hidden';
  }

  private enableBodyScroll(): void {
    document.body.style.overflow = '';
  }
}
