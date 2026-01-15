import { Component, computed, input, output, forwardRef, signal } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'ds-checkbox',
  standalone: true,
  templateUrl: './checkbox.component.html',
  styleUrl: './checkbox.component.css',
  host: {
    'class': 'ds-checkbox-wrapper'
  },
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CheckboxComponent),
      multi: true
    }
  ]
})
export class CheckboxComponent implements ControlValueAccessor {
  // Inputs
  readonly label = input<string | undefined>(undefined);
  readonly description = input<string | undefined>(undefined);
  readonly disabled = input(false);
  readonly indeterminate = input(false);
  readonly size = input<'sm' | 'md' | 'lg'>('md');
  readonly id = input<string>(`ds-checkbox-${Math.random().toString(36).slice(2, 9)}`);

  // Internal state
  readonly checked = signal(false);
  readonly focused = signal(false);

  // Outputs
  readonly change = output<boolean>();

  // Computed
  readonly checkboxClasses = computed(() => {
    const classes = ['ds-checkbox-control', `ds-checkbox-control--${this.size()}`];

    if (this.checked()) classes.push('ds-checkbox-control--checked');
    if (this.indeterminate()) classes.push('ds-checkbox-control--indeterminate');
    if (this.disabled()) classes.push('ds-checkbox-control--disabled');
    if (this.focused()) classes.push('ds-checkbox-control--focused');

    return classes.join(' ');
  });

  // ControlValueAccessor
  private onChange: (value: boolean) => void = () => {};
  private onTouched: () => void = () => {};

  writeValue(value: boolean): void {
    this.checked.set(!!value);
  }

  registerOnChange(fn: (value: boolean) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {}

  // Event handlers
  toggle(): void {
    if (this.disabled()) return;

    const newValue = !this.checked();
    this.checked.set(newValue);
    this.onChange(newValue);
    this.change.emit(newValue);
  }

  onFocus(): void {
    this.focused.set(true);
  }

  onBlur(): void {
    this.focused.set(false);
    this.onTouched();
  }

  onKeydown(event: KeyboardEvent): void {
    if (event.key === ' ' || event.key === 'Enter') {
      event.preventDefault();
      this.toggle();
    }
  }
}
