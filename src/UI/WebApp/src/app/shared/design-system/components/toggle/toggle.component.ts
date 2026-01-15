import { Component, computed, input, output, forwardRef, signal } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'ds-toggle',
  standalone: true,
  templateUrl: './toggle.component.html',
  styleUrl: './toggle.component.css',
  host: {
    'class': 'ds-toggle-wrapper'
  },
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ToggleComponent),
      multi: true
    }
  ]
})
export class ToggleComponent implements ControlValueAccessor {
  // Inputs
  readonly label = input<string | undefined>(undefined);
  readonly description = input<string | undefined>(undefined);
  readonly disabled = input(false);
  readonly size = input<'sm' | 'md' | 'lg'>('md');
  readonly labelPosition = input<'left' | 'right'>('right');
  readonly id = input<string>(`ds-toggle-${Math.random().toString(36).slice(2, 9)}`);

  // Internal state
  readonly checked = signal(false);
  readonly focused = signal(false);

  // Outputs
  readonly change = output<boolean>();

  // Computed
  readonly toggleClasses = computed(() => {
    const classes = ['ds-toggle-track', `ds-toggle-track--${this.size()}`];

    if (this.checked()) classes.push('ds-toggle-track--checked');
    if (this.disabled()) classes.push('ds-toggle-track--disabled');
    if (this.focused()) classes.push('ds-toggle-track--focused');

    return classes.join(' ');
  });

  readonly thumbClasses = computed(() => {
    const classes = ['ds-toggle-thumb', `ds-toggle-thumb--${this.size()}`];

    if (this.checked()) classes.push('ds-toggle-thumb--checked');

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
