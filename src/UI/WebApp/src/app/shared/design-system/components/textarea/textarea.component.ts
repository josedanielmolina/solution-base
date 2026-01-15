import { Component, computed, input, output, forwardRef, signal } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'ds-textarea',
  standalone: true,
  templateUrl: './textarea.component.html',
  styleUrl: './textarea.component.css',
  host: {
    'class': 'ds-textarea-wrapper'
  },
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TextareaComponent),
      multi: true
    }
  ]
})
export class TextareaComponent implements ControlValueAccessor {
  // Inputs
  readonly placeholder = input('');
  readonly label = input<string | undefined>(undefined);
  readonly hint = input<string | undefined>(undefined);
  readonly error = input<string | undefined>(undefined);
  readonly disabled = input(false);
  readonly readonly = input(false);
  readonly required = input(false);
  readonly rows = input(4);
  readonly maxLength = input<number | undefined>(undefined);
  readonly showCount = input(false);
  readonly autoResize = input(false);
  readonly id = input<string>(`ds-textarea-${Math.random().toString(36).slice(2, 9)}`);

  // Internal state
  readonly value = signal('');
  readonly focused = signal(false);
  readonly touched = signal(false);

  // Outputs
  readonly valueChange = output<string>();
  readonly focus = output<FocusEvent>();
  readonly blur = output<FocusEvent>();

  // Computed
  readonly hasError = computed(() => !!this.error() && this.touched());
  readonly charCount = computed(() => this.value().length);
  readonly charCountText = computed(() => {
    const max = this.maxLength();
    return max ? `${this.charCount()}/${max}` : `${this.charCount()}`;
  });

  readonly wrapperClasses = computed(() => {
    const classes = ['ds-textarea-field'];
    
    if (this.focused()) classes.push('ds-textarea-field--focused');
    if (this.hasError()) classes.push('ds-textarea-field--error');
    if (this.disabled()) classes.push('ds-textarea-field--disabled');
    if (this.readonly()) classes.push('ds-textarea-field--readonly');
    
    return classes.join(' ');
  });

  // ControlValueAccessor
  private onChange: (value: string) => void = () => {};
  private onTouched: () => void = () => {};

  writeValue(value: string): void {
    this.value.set(value ?? '');
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {}

  // Event handlers
  onInput(event: Event): void {
    const target = event.target as HTMLTextAreaElement;
    this.value.set(target.value);
    this.onChange(target.value);
    this.valueChange.emit(target.value);

    if (this.autoResize()) {
      this.adjustHeight(target);
    }
  }

  onFocus(event: FocusEvent): void {
    this.focused.set(true);
    this.focus.emit(event);
  }

  onBlur(event: FocusEvent): void {
    this.focused.set(false);
    this.touched.set(true);
    this.onTouched();
    this.blur.emit(event);
  }

  private adjustHeight(element: HTMLTextAreaElement): void {
    element.style.height = 'auto';
    element.style.height = element.scrollHeight + 'px';
  }
}
