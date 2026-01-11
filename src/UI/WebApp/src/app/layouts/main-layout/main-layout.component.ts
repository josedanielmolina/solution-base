import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NotificationComponent } from '../../core/components/notification/notification.component';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterModule, NotificationComponent],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MainLayoutComponent {}
