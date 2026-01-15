import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'ds-avatar',
  standalone: true,
  templateUrl: './avatar.component.html',
  styleUrl: './avatar.component.css',
  host: {
    'class': 'ds-avatar-wrapper'
  }
})
export class AvatarComponent {
  // Inputs
  readonly src = input<string | undefined>(undefined);
  readonly alt = input('');
  readonly name = input<string | undefined>(undefined);
  readonly size = input<'xs' | 'sm' | 'md' | 'lg' | 'xl' | '2xl'>('md');
  readonly shape = input<'circle' | 'square' | 'rounded'>('circle');
  readonly status = input<'online' | 'offline' | 'away' | 'busy' | undefined>(undefined);

  // Computed
  readonly initials = computed(() => {
    const n = this.name();
    if (!n) return '';
    const parts = n.trim().split(/\s+/);
    if (parts.length >= 2) {
      return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase();
    }
    return n.substring(0, 2).toUpperCase();
  });

  readonly avatarClasses = computed(() => {
    const classes = [
      'ds-avatar',
      `ds-avatar--${this.size()}`,
      `ds-avatar--${this.shape()}`
    ];
    return classes.join(' ');
  });

  readonly statusClasses = computed(() => {
    return ['ds-avatar-status', `ds-avatar-status--${this.status()}`].join(' ');
  });

  readonly showImage = computed(() => !!this.src());
  readonly showInitials = computed(() => !this.src() && !!this.name());
  readonly showFallback = computed(() => !this.src() && !this.name());
}
