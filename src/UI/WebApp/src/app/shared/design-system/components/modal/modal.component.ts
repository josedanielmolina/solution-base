import { Component, computed, input, output, signal, ElementRef, HostListener, OnInit, OnDestroy } from '@angular/core';

@Component({
  selector: 'ds-modal',
  standalone: true,
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.css',
  host: {
    'class': 'ds-modal-host'
  }
})
export class ModalComponent implements OnInit, OnDestroy {
  // Inputs
  readonly isOpen = input(false);
  readonly title = input<string | undefined>(undefined);
  readonly size = input<'sm' | 'md' | 'lg' | 'xl' | 'full'>('md');
  readonly closeOnBackdrop = input(true);
  readonly closeOnEscape = input(true);
  readonly showClose = input(true);
  readonly ariaLabel = input<string | undefined>(undefined);

  // Outputs
  readonly close = output<void>();
  readonly afterOpen = output<void>();
  readonly afterClose = output<void>();

  // Internal state
  readonly isVisible = signal(false);
  readonly isAnimating = signal(false);

  // Computed
  readonly modalClasses = computed(() => {
    const classes = ['ds-modal', `ds-modal--${this.size()}`];
    if (this.isVisible()) classes.push('ds-modal--open');
    return classes.join(' ');
  });

  readonly overlayClasses = computed(() => {
    const classes = ['ds-modal-overlay'];
    if (this.isVisible()) classes.push('ds-modal-overlay--visible');
    return classes.join(' ');
  });

  ngOnInit(): void {
    if (this.isOpen()) {
      this.open();
    }
  }

  ngOnDestroy(): void {
    this.enableBodyScroll();
  }

  open(): void {
    this.isVisible.set(true);
    this.disableBodyScroll();
    this.afterOpen.emit();
  }

  closeModal(): void {
    this.isVisible.set(false);
    this.enableBodyScroll();
    this.close.emit();
    this.afterClose.emit();
  }

  onBackdropClick(event: MouseEvent): void {
    if (this.closeOnBackdrop() && event.target === event.currentTarget) {
      this.closeModal();
    }
  }

  @HostListener('document:keydown.escape')
  onEscapeKey(): void {
    if (this.closeOnEscape() && this.isVisible()) {
      this.closeModal();
    }
  }

  private disableBodyScroll(): void {
    document.body.style.overflow = 'hidden';
  }

  private enableBodyScroll(): void {
    document.body.style.overflow = '';
  }
}
