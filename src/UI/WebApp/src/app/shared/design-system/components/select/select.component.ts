import { Component, computed, input, output, forwardRef, signal, contentChildren, AfterContentInit, ElementRef, HostListener } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { SelectOptionComponent } from './select-option.component';

export interface SelectOption {
  value: string;
  label: string;
  disabled?: boolean;
}

@Component({
  selector: 'ds-select',
  standalone: true,
  templateUrl: './select.component.html',
  styleUrl: './select.component.css',
  host: {
    'class': 'ds-select-wrapper'
  },
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SelectComponent),
      multi: true
    }
  ]
})
export class SelectComponent implements ControlValueAccessor, AfterContentInit {
  // Inputs
  readonly options = input<SelectOption[]>([]);
  readonly placeholder = input('Seleccionar...');
  readonly label = input<string | undefined>(undefined);
  readonly hint = input<string | undefined>(undefined);
  readonly error = input<string | undefined>(undefined);
  readonly disabled = input(false);
  readonly required = input(false);
  readonly searchable = input(false);
  readonly clearable = input(false);
  readonly size = input<'sm' | 'md' | 'lg'>('md');
  readonly id = input<string>(`ds-select-${Math.random().toString(36).slice(2, 9)}`);

  // Content children (for template-based options)
  readonly optionComponents = contentChildren(SelectOptionComponent);

  // Internal state
  readonly value = signal<string | null>(null);
  readonly isOpen = signal(false);
  readonly focused = signal(false);
  readonly touched = signal(false);
  readonly searchQuery = signal('');
  readonly highlightedIndex = signal(-1);

  // Outputs
  readonly valueChange = output<string | null>();
  readonly openChange = output<boolean>();

  // Computed
  readonly hasError = computed(() => !!this.error() && this.touched());

  readonly allOptions = computed(() => {
    const inputOptions = this.options();
    const templateOptions = this.optionComponents().map(opt => ({
      value: opt.value(),
      label: opt.label() || opt.value(),
      disabled: opt.disabled()
    }));
    return [...inputOptions, ...templateOptions];
  });

  readonly filteredOptions = computed(() => {
    const query = this.searchQuery().toLowerCase();
    if (!query) return this.allOptions();
    return this.allOptions().filter(opt =>
      opt.label.toLowerCase().includes(query)
    );
  });

  readonly selectedOption = computed(() => {
    return this.allOptions().find(opt => opt.value === this.value());
  });

  readonly displayValue = computed(() => {
    return this.selectedOption()?.label || '';
  });

  readonly selectClasses = computed(() => {
    const classes = ['ds-select', `ds-select--${this.size()}`];

    if (this.isOpen()) classes.push('ds-select--open');
    if (this.focused()) classes.push('ds-select--focused');
    if (this.hasError()) classes.push('ds-select--error');
    if (this.disabled()) classes.push('ds-select--disabled');
    if (this.value()) classes.push('ds-select--has-value');

    return classes.join(' ');
  });

  constructor(private elementRef: ElementRef) {}

  ngAfterContentInit(): void {}

  // ControlValueAccessor
  private onChange: (value: string | null) => void = () => {};
  private onTouched: () => void = () => {};

  writeValue(value: string): void {
    this.value.set(value);
  }

  registerOnChange(fn: (value: string | null) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {}

  // Event handlers
  toggleDropdown(): void {
    if (this.disabled()) return;

    if (this.isOpen()) {
      this.close();
    } else {
      this.open();
    }
  }

  open(): void {
    this.isOpen.set(true);
    this.openChange.emit(true);
    this.highlightedIndex.set(
      this.filteredOptions().findIndex(opt => opt.value === this.value())
    );
  }

  close(): void {
    this.isOpen.set(false);
    this.searchQuery.set('');
    this.openChange.emit(false);
  }

  selectOption(option: SelectOption): void {
    if (option.disabled) return;

    this.value.set(option.value);
    this.onChange(option.value);
    this.valueChange.emit(option.value);
    this.close();
  }

  clear(event: Event): void {
    event.stopPropagation();
    this.value.set(null);
    this.onChange(null);
    this.valueChange.emit(null);
  }

  onSearchInput(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.searchQuery.set(target.value);
    this.highlightedIndex.set(0);
  }

  onFocus(): void {
    this.focused.set(true);
  }

  onBlur(): void {
    this.focused.set(false);
    this.touched.set(true);
    this.onTouched();
  }

  onKeydown(event: KeyboardEvent): void {
    const options = this.filteredOptions();

    switch (event.key) {
      case 'Enter':
      case ' ':
        event.preventDefault();
        if (this.isOpen() && this.highlightedIndex() >= 0) {
          this.selectOption(options[this.highlightedIndex()]);
        } else {
          this.toggleDropdown();
        }
        break;
      case 'ArrowDown':
        event.preventDefault();
        if (!this.isOpen()) {
          this.open();
        } else {
          this.highlightedIndex.set(
            Math.min(this.highlightedIndex() + 1, options.length - 1)
          );
        }
        break;
      case 'ArrowUp':
        event.preventDefault();
        this.highlightedIndex.set(Math.max(this.highlightedIndex() - 1, 0));
        break;
      case 'Escape':
        this.close();
        break;
      case 'Tab':
        this.close();
        break;
    }
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event): void {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.close();
    }
  }
}
