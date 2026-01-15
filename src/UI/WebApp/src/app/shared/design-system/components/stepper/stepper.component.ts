import { Component, computed, input, output, signal } from '@angular/core';

export interface StepItem {
  id: string;
  label: string;
  description?: string;
  optional?: boolean;
  completed?: boolean;
  error?: boolean;
}

@Component({
  selector: 'ds-stepper',
  standalone: true,
  templateUrl: './stepper.component.html',
  styleUrl: './stepper.component.css',
  host: {
    'class': 'ds-stepper-wrapper'
  }
})
export class StepperComponent {
  // Inputs
  readonly steps = input.required<StepItem[]>();
  readonly activeStep = input(0);
  readonly orientation = input<'horizontal' | 'vertical'>('horizontal');
  readonly size = input<'sm' | 'md' | 'lg'>('md');
  readonly clickable = input(false);

  // Internal state
  private readonly internalStep = signal<number | null>(null);

  // Outputs
  readonly stepChange = output<number>();

  // Computed
  readonly currentStep = computed(() => {
    return this.internalStep() ?? this.activeStep();
  });

  readonly stepperClasses = computed(() => {
    const classes = [
      'ds-stepper',
      `ds-stepper--${this.orientation()}`,
      `ds-stepper--${this.size()}`
    ];
    return classes.join(' ');
  });

  getStepClasses(index: number, step: StepItem): string {
    const classes = ['ds-step'];

    if (index < this.currentStep() || step.completed) {
      classes.push('ds-step--completed');
    } else if (index === this.currentStep()) {
      classes.push('ds-step--active');
    }

    if (step.error) classes.push('ds-step--error');
    if (this.clickable()) classes.push('ds-step--clickable');

    return classes.join(' ');
  }

  goToStep(index: number): void {
    if (!this.clickable()) return;

    this.internalStep.set(index);
    this.stepChange.emit(index);
  }

  isLastStep(index: number): boolean {
    return index === this.steps().length - 1;
  }
}
