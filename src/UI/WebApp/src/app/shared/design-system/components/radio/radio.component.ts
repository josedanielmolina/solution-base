import { Component, computed, input, output, forwardRef, signal } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'ds-radio',
  standalone: true,
  templateUrl: './radio.component.html',
  styleUrl: './radio.component.css',
  host: {
    'class': 'ds-radio-wrapper'
  },
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => RadioComponent),
      multi: true
    }
  ]
})
export class RadioComponent implements ControlValueAccessor {
  // Inputs
  readonly value = input.required<string>();
  readonly name = input.required<string>();
  readonly label = input<string | undefined>(undefined);
  readonly description = input<string | undefined>(undefined);
  readonly disabled = input(false);
  readonly size = input<'sm' | 'md' | 'lg'>('md');
  readonly id = input<string>(`ds-radio-${Math.random().toString(36).slice(2, 9)}`);

  // Internal state
  readonly checked = signal(false);
  readonly focused = signal(false);

  // Outputs
  readonly change = output<string>();

  // Computed
  readonly radioClasses = computed(() => {
    const classes = ['ds-radio-control', `ds-radio-control--${this.size()}`];

    if (this.checked()) classes.push('ds-radio-control--checked');
    if (this.disabled()) classes.push('ds-radio-control--disabled');
    if (this.focused()) classes.push('ds-radio-control--focused');

    return classes.join(' ');
  });

  // ControlValueAccessor
  private onChange: (value: string) => void = () => {};
  private onTouched: () => void = () => {};

  writeValue(value: string): void {
    this.checked.set(value === this.value());
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {}

  // Event handlers
  select(): void {
    if (this.disabled() || this.checked()) return;

    this.checked.set(true);
    this.onChange(this.value());
    this.change.emit(this.value());
  }

  onFocus(): void {
    this.focused.set(true);
  }

  onBlur(): void {
    this.focused.set(false);
    this.onTouched();
  }
}
