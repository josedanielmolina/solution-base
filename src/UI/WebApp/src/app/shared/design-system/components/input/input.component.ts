import { Component, computed, input, output, forwardRef, signal, effect } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

export type InputType = 'text' | 'email' | 'password' | 'number' | 'search' | 'tel' | 'url';
export type InputSize = 'sm' | 'md' | 'lg';

@Component({
  selector: 'ds-input',
  standalone: true,
  templateUrl: './input.component.html',
  styleUrl: './input.component.css',
  host: {
    'class': 'ds-input-wrapper'
  },
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputComponent),
      multi: true
    }
  ]
})
export class InputComponent implements ControlValueAccessor {
  // Inputs
  readonly type = input<InputType>('text');
  readonly size = input<InputSize>('md');
  readonly placeholder = input('');
  readonly label = input<string | undefined>(undefined);
  readonly hint = input<string | undefined>(undefined);
  readonly error = input<string | undefined>(undefined);
  readonly disabled = input(false);
  readonly readonly = input(false);
  readonly required = input(false);
  readonly autocomplete = input<string>('off');
  readonly id = input<string>(`ds-input-${Math.random().toString(36).slice(2, 9)}`);
  readonly showPasswordToggle = input(false);

  // Internal state
  readonly value = signal('');
  readonly focused = signal(false);
  readonly touched = signal(false);
  readonly passwordVisible = signal(false);

  // Outputs
  readonly valueChange = output<string>();
  readonly focus = output<FocusEvent>();
  readonly blur = output<FocusEvent>();

  // Computed
  readonly hasError = computed(() => !!this.error() && this.touched());
  
  readonly inputType = computed(() => {
    if (this.type() === 'password' && this.passwordVisible()) {
      return 'text';
    }
    return this.type();
  });

  readonly wrapperClasses = computed(() => {
    const classes = ['ds-input-field', `ds-input-field--${this.size()}`];
    
    if (this.focused()) classes.push('ds-input-field--focused');
    if (this.hasError()) classes.push('ds-input-field--error');
    if (this.disabled()) classes.push('ds-input-field--disabled');
    if (this.readonly()) classes.push('ds-input-field--readonly');
    
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

  setDisabledState(isDisabled: boolean): void {
    // Handled by input signal
  }

  // Event handlers
  onInput(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.value.set(target.value);
    this.onChange(target.value);
    this.valueChange.emit(target.value);
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

  togglePasswordVisibility(): void {
    this.passwordVisible.update(v => !v);
  }
}
