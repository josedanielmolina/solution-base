import { Component, computed, input, output, signal, OnInit, OnDestroy } from '@angular/core';

@Component({
  selector: 'ds-toast',
  standalone: true,
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.css',
  host: {
    'class': 'ds-toast-wrapper',
    'role': 'alert',
    '[attr.aria-live]': 'ariaLive()'
  }
})
export class ToastComponent implements OnInit, OnDestroy {
  // Inputs
  readonly variant = input<'success' | 'warning' | 'danger' | 'info'>('info');
  readonly title = input<string | undefined>(undefined);
  readonly message = input<string | undefined>(undefined);
  readonly dismissible = input(true);
  readonly duration = input<number | null>(5000); // null = no auto-dismiss
  readonly icon = input(true);
  readonly position = input<'top-right' | 'top-left' | 'bottom-right' | 'bottom-left' | 'top-center' | 'bottom-center'>('top-right');

  // Internal state
  readonly visible = signal(true);
  private timeoutId: ReturnType<typeof setTimeout> | null = null;

  // Outputs
  readonly dismiss = output<void>();

  // Computed
  readonly toastClasses = computed(() => {
    const classes = ['ds-toast', `ds-toast--${this.variant()}`];
    if (!this.visible()) classes.push('ds-toast--hidden');
    return classes.join(' ');
  });

  readonly ariaLive = computed(() => {
    return this.variant() === 'danger' ? 'assertive' : 'polite';
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

  ngOnInit(): void {
    const duration = this.duration();
    if (duration && duration > 0) {
      this.timeoutId = setTimeout(() => this.close(), duration);
    }
  }

  ngOnDestroy(): void {
    if (this.timeoutId) {
      clearTimeout(this.timeoutId);
    }
  }

  close(): void {
    this.visible.set(false);
    this.dismiss.emit();
  }
}
