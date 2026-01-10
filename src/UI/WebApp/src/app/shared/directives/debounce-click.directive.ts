import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
  selector: '[appDebounceClick]',
  standalone: true
})
export class DebounceClickDirective {
  @Input() debounceTime = 500;
  @Input() appDebounceClick: (() => void) | undefined;

  private timeoutId: any;

  constructor(private el: ElementRef) {}

  @HostListener('click', ['$event'])
  onClick(event: Event): void {
    event.preventDefault();
    event.stopPropagation();

    clearTimeout(this.timeoutId);

    this.timeoutId = setTimeout(() => {
      if (this.appDebounceClick) {
        this.appDebounceClick();
      }
    }, this.debounceTime);
  }
}
