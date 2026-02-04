import { Component, EventEmitter, inject, Input, OnInit, Output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { EstablishmentService } from '../../services/establishment.service';
import { Court, COURT_TYPES } from '../../models/establishment.model';

@Component({
    selector: 'app-court-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './court-form.component.html',
    styleUrl: './court-form.component.css'
})
export class CourtFormComponent implements OnInit {
    private readonly fb = inject(FormBuilder);
    private readonly service = inject(EstablishmentService);

    @Input() establishmentId!: number;
    @Input() court: Court | null = null;
    @Output() saved = new EventEmitter<void>();
    @Output() cancelled = new EventEmitter<void>();

    form!: FormGroup;
    saving = signal(false);
    error = signal<string | null>(null);
    courtTypes = COURT_TYPES;

    get isEdit(): boolean {
        return !!this.court;
    }

    ngOnInit(): void {
        this.initForm();
        if (this.court) {
            this.form.patchValue({
                name: this.court.name,
                courtType: this.court.courtType,
                isActive: this.court.isActive
            });
        }
    }

    private initForm(): void {
        this.form = this.fb.group({
            name: ['', [Validators.required, Validators.maxLength(50)]],
            courtType: [1, Validators.required],
            isActive: [true]
        });
    }

    onSubmit(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        this.saving.set(true);
        this.error.set(null);

        const data = {
            ...this.form.value,
            courtType: parseInt(this.form.value.courtType, 10)
        };

        const operation = this.isEdit
            ? this.service.updateCourt(this.establishmentId, this.court!.id, data)
            : this.service.createCourt(this.establishmentId, data);

        operation.subscribe({
            next: () => {
                this.saving.set(false);
                this.saved.emit();
            },
            error: (err) => {
                this.saving.set(false);
                this.error.set(err.error?.message || 'Error al guardar');
                console.error(err);
            }
        });
    }

    onCancel(): void {
        this.cancelled.emit();
    }
}
