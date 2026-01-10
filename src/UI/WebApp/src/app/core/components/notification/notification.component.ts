import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="notification-container">
      @for (notification of notificationService.getNotifications()(); track notification.id) {
        <div
          class="notification"
          [class.success]="notification.type === 'success'"
          [class.error]="notification.type === 'error'"
          [class.warning]="notification.type === 'warning'"
          [class.info]="notification.type === 'info'"
        >
          <span class="message">{{ notification.message }}</span>
          <button class="close-btn" (click)="notificationService.remove(notification.id)">
            ×
          </button>
        </div>
      }
    </div>
  `,
  styles: [`
    .notification-container {
      position: fixed;
      top: 20px;
      right: 20px;
      z-index: 10000;
      display: flex;
      flex-direction: column;
      gap: 10px;
      max-width: 400px;
    }

    .notification {
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 15px 20px;
      border-radius: 4px;
      box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
      color: white;
      animation: slideIn 0.3s ease-out;
    }

    @keyframes slideIn {
      from {
        transform: translateX(100%);
        opacity: 0;
      }
      to {
        transform: translateX(0);
        opacity: 1;
      }
    }

    .notification.success {
      background-color: #28a745;
    }

    .notification.error {
      background-color: #dc3545;
    }

    .notification.warning {
      background-color: #ffc107;
      color: #333;
    }

    .notification.info {
      background-color: #17a2b8;
    }

    .message {
      flex: 1;
      margin-right: 10px;
    }

    .close-btn {
      background: none;
      border: none;
      color: inherit;
      font-size: 24px;
      cursor: pointer;
      padding: 0;
      width: 24px;
      height: 24px;
      display: flex;
      align-items: center;
      justify-content: center;
      opacity: 0.7;
      transition: opacity 0.2s;
    }

    .close-btn:hover {
      opacity: 1;
    }
  `]
})
export class NotificationComponent {
  notificationService = inject(NotificationService);
}
