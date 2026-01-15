import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  ButtonComponent,
  InputComponent,
  TextareaComponent,
  CheckboxComponent,
  RadioComponent,
  ToggleComponent,
  SelectComponent,
  AlertComponent,
  SpinnerComponent,
  ProgressComponent,
  SkeletonComponent,
  CardComponent,
  BadgeComponent,
  AvatarComponent,
  DividerComponent,
  EmptyStateComponent
} from '../../shared/design-system';

@Component({
  selector: 'app-component-kit-layout',
  standalone: true,
  imports: [
    FormsModule,
    ButtonComponent,
    InputComponent,
    TextareaComponent,
    CheckboxComponent,
    RadioComponent,
    ToggleComponent,
    SelectComponent,
    AlertComponent,
    SpinnerComponent,
    ProgressComponent,
    SkeletonComponent,
    CardComponent,
    BadgeComponent,
    AvatarComponent,
    DividerComponent,
    EmptyStateComponent
  ],
  templateUrl: './component-kit-layout.component.html',
  styleUrls: ['./component-kit-layout.component.css']
})
export class ComponentKitLayoutComponent {
  // Demo states
  isLoading = signal(false);
  checkboxValue = signal(false);
  toggleValue = signal(true);
  radioValue = signal('option1');
  progressValue = signal(65);

  selectOptions = [
    { value: 'es', label: 'España' },
    { value: 'mx', label: 'México' },
    { value: 'ar', label: 'Argentina' },
    { value: 'co', label: 'Colombia' },
    { value: 'cl', label: 'Chile' },
    { value: 'pe', label: 'Perú' },
    { value: 'ec', label: 'Ecuador' },
    { value: 've', label: 'Venezuela' }
  ];

  categoryOptions = [
    { value: 'tech', label: 'Tecnología' },
    { value: 'sports', label: 'Deportes' },
    { value: 'music', label: 'Música' },
    { value: 'art', label: 'Arte' }
  ];

  simulateLoading(): void {
    this.isLoading.set(true);
    setTimeout(() => this.isLoading.set(false), 2000);
  }
}
