import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NotificationComponent } from '../../core/components/notification/notification.component';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterModule, NotificationComponent],
  template: `
    <div class="main-layout">
      <app-notification></app-notification>

      <header class="header">
        <div class="container">
          <h1 class="logo">Solution Base</h1>
          <nav class="nav">
            <a routerLink="/users" routerLinkActive="active">Users</a>
          </nav>
        </div>
      </header>

      <main class="main-content">
        <router-outlet></router-outlet>
      </main>

      <footer class="footer">
        <div class="container">
          <p>&copy; 2026 Solution Base. All rights reserved.</p>
        </div>
      </footer>
    </div>
  `,
  styles: [`
    .main-layout {
      min-height: 100vh;
      display: flex;
      flex-direction: column;
    }

    .header {
      background-color: #343a40;
      color: white;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .container {
      max-width: 1200px;
      margin: 0 auto;
      padding: 0 20px;
    }

    .header .container {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 1rem 20px;
    }

    .logo {
      margin: 0;
      font-size: 1.5rem;
      font-weight: 600;
    }

    .nav {
      display: flex;
      gap: 20px;
    }

    .nav a {
      color: white;
      text-decoration: none;
      padding: 8px 16px;
      border-radius: 4px;
      transition: background-color 0.3s;
    }

    .nav a:hover {
      background-color: rgba(255, 255, 255, 0.1);
    }

    .nav a.active {
      background-color: #007bff;
    }

    .main-content {
      flex: 1;
      background-color: #f8f9fa;
      padding: 20px 0;
    }

    .footer {
      background-color: #f8f9fa;
      border-top: 1px solid #dee2e6;
      padding: 1rem 0;
      text-align: center;
    }

    .footer p {
      margin: 0;
      color: #6c757d;
    }
  `]
})
export class MainLayoutComponent {}
