import { Component, ChangeDetectionStrategy } from '@angular/core';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';

@Component({
  selector: 'app-root',
  imports: [MainLayoutComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: '<app-main-layout />',
  styleUrl: './app.css'
})
export class App {
}
