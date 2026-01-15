import { Component, input } from '@angular/core';

@Component({
  selector: 'ds-accordion-item',
  standalone: true,
  template: '<ng-content />'
})
export class AccordionItemComponent {
  readonly id = input.required<string>();
  readonly title = input.required<string>();
  readonly disabled = input(false);
  readonly expanded = input(false);
}
