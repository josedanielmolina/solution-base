import { Directive, ElementRef, HostListener, input, inject } from '@angular/core';

@Directive({
  selector: '[appDebounceClick]',
  standalone: true
})
export class DebounceClickDirective {
  private el = inject(ElementRef);

  debounceTime = input<number>(500);
  appDebounceClick = input<(() => void) | undefined>(undefined);

  private timeoutId: any;

  @HostListener('click', ['$event'])
  onClick(event: Event): void {
    event.preventDefault();
    event.stopPropagation();

    clearTimeout(this.timeoutId);

    this.timeoutId = setTimeout(() => {
      const clickHandler = this.appDebounceClick();
      if (clickHandler) {
        clickHandler();
      }
    }, this.debounceTime());
  }
}
