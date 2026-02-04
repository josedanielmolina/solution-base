import { Component, ChangeDetectionStrategy, inject, signal, computed } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '@core/services/auth.service';

interface MenuItem {
  label: string;
  icon: string;
  route: string;
  roles?: string[]; // If empty or undefined, visible to all
}

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MainLayoutComponent {
  private authService = inject(AuthService);

  currentUser = this.authService.currentUser;
  showUserMenu = signal<boolean>(false);

  toggleUserMenu(): void {
    this.showUserMenu.update(v => !v);
  }

  closeUserMenu(): void {
    this.showUserMenu.set(false);
  }

  private allMenuItems: MenuItem[] = [
    {
      label: 'Eventos',
      icon: '📅',
      route: '/app/events'
      // No roles = visible to all
    },
    {
      label: 'Usuarios',
      icon: '👥',
      route: '/app/users',
      roles: ['PlatformAdmin']
    },
    {
      label: 'Roles',
      icon: '🔐',
      route: '/app/admin/roles',
      roles: ['PlatformAdmin']
    },
    {
      label: 'Países',
      icon: '🌎',
      route: '/app/admin/countries',
      roles: ['PlatformAdmin']
    },
    {
      label: 'Ciudades',
      icon: '🏙️',
      route: '/app/admin/cities',
      roles: ['PlatformAdmin']
    },
    {
      label: 'Categorías',
      icon: '🏆',
      route: '/app/admin/categories',
      roles: ['PlatformAdmin']
    },
    {
      label: 'Establecimientos',
      icon: '🏢',
      route: '/app/admin/establishments',
      roles: ['PlatformAdmin']
    }
  ];

  // Filter menu items based on user roles
  menuItems = computed(() => {
    const user = this.currentUser();
    if (!user) return [];

    return this.allMenuItems.filter(item => {
      // If no roles specified, show to everyone
      if (!item.roles || item.roles.length === 0) {
        return true;
      }
      // Check if user has any of the required roles
      return item.roles.some(role => user.roles.includes(role));
    });
  });

  logout(): void {
    this.authService.logout();
  }
}

