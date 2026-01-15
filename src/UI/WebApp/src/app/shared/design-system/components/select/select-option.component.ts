import { Component, input } from '@angular/core';

@Component({
  selector: 'ds-select-option',
  standalone: true,
  template: '<ng-content />'
})
export class SelectOptionComponent {
  readonly value = input.required<string>();
  readonly label = input<string | undefined>(undefined);
  readonly disabled = input(false);
}
