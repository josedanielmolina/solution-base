import { Component, EventEmitter, inject, Input, OnInit, Output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule, Validators } from '@angular/forms';
import { EstablishmentService } from '../../services/establishment.service';
import { Schedule, DAYS_OF_WEEK } from '../../models/establishment.model';

@Component({
    selector: 'app-schedule-form',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './schedule-form.component.html',
    styleUrl: './schedule-form.component.css'
})
export class ScheduleFormComponent implements OnInit {
    private readonly fb = inject(FormBuilder);
    private readonly service = inject(EstablishmentService);

    @Input() establishmentId!: number;
    @Input() scheduleType: number = 1; // 1=Continuous, 2=Blocks
    @Input() schedules: Schedule[] = [];
    @Output() saved = new EventEmitter<void>();
    @Output() cancelled = new EventEmitter<void>();

    form!: FormGroup;
    saving = signal(false);
    error = signal<string | null>(null);
    daysOfWeek = DAYS_OF_WEEK;

    get schedulesArray(): FormArray {
        return this.form.get('schedules') as FormArray;
    }

    ngOnInit(): void {
        this.initForm();
    }

    private initForm(): void {
        this.form = this.fb.group({
            schedules: this.fb.array([])
        });

        // Initialize with existing schedules or default days
        if (this.schedules.length > 0) {
            this.schedules.forEach(s => this.addScheduleRow(s));
        } else {
            // Add default rows for each day (Mon-Sun)
            for (let day = 1; day <= 7; day++) {
                this.addScheduleRow({ dayOfWeek: day, openTime: '08:00', closeTime: '22:00', blockNumber: 1 });
            }
        }
    }

    private addScheduleRow(data: Partial<Schedule> = {}): void {
        const row = this.fb.group({
            dayOfWeek: [data.dayOfWeek || 1, Validators.required],
            openTime: [data.openTime || '08:00', Validators.required],
            closeTime: [data.closeTime || '22:00', Validators.required],
            blockNumber: [data.blockNumber || 1],
            enabled: [true]
        });
        this.schedulesArray.push(row);
    }

    addBlock(dayOfWeek: number): void {
        const lastBlock = this.schedulesArray.controls
            .filter((c: any) => c.value.dayOfWeek === dayOfWeek)
            .reduce((max: number, c: any) => Math.max(max, c.value.blockNumber), 0);

        this.addScheduleRow({
            dayOfWeek,
            openTime: '14:00',
            closeTime: '22:00',
            blockNumber: lastBlock + 1
        });
    }

    removeBlock(index: number): void {
        this.schedulesArray.removeAt(index);
    }

    getDayName(day: number): string {
        return this.daysOfWeek.find(d => d.value === day)?.label || '';
    }

    getSchedulesForDay(day: number): { control: FormGroup; index: number }[] {
        return this.schedulesArray.controls
            .map((c, i) => ({ control: c as FormGroup, index: i }))
            .filter(item => item.control.value.dayOfWeek === day);
    }

    onSubmit(): void {
        if (this.form.invalid) return;

        this.saving.set(true);
        this.error.set(null);

        const schedules = this.schedulesArray.controls
            .filter((c: any) => c.value.enabled)
            .map((c: any) => ({
                dayOfWeek: c.value.dayOfWeek,
                openTime: c.value.openTime,
                closeTime: c.value.closeTime,
                blockNumber: c.value.blockNumber
            }));

        this.service.setSchedules(this.establishmentId, {
            scheduleType: this.scheduleType,
            schedules
        }).subscribe({
            next: () => {
                this.saving.set(false);
                this.saved.emit();
            },
            error: (err: any) => {
                this.saving.set(false);
                this.error.set(err.error?.message || 'Error al guardar horarios');
                console.error(err);
            }
        });
    }

    onCancel(): void {
        this.cancelled.emit();
    }
}
