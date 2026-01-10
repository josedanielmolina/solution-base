import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [],
  template: `
    <footer>
      <p>&copy; 2026 Solution Base. All rights reserved.</p>
    </footer>
  `,
  styles: [`
    footer {
      background-color: #f5f5f5;
      padding: 1rem;
      text-align: center;
      border-top: 1px solid #ddd;
    }

    p {
      margin: 0;
      color: #666;
    }
  `]
})
export class FooterComponent {}
