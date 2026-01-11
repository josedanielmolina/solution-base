import { Directive, ElementRef, input, inject } from '@angular/core';

@Directive({
  selector: '[appAutoFocus]',
  standalone: true
})
export class AutoFocusDirective {
  private el = inject(ElementRef);

  appAutoFocus = input<boolean>(true);

  ngAfterViewInit(): void {
    if (this.appAutoFocus()) {
      setTimeout(() => {
        this.el.nativeElement.focus();
      }, 100);
    }
  }
}
