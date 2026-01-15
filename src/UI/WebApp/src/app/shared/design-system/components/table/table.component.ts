import { Component, computed, input, output, signal, contentChildren, AfterContentInit } from '@angular/core';

export interface TableColumn {
  key: string;
  header: string;
  sortable?: boolean;
  width?: string;
  align?: 'left' | 'center' | 'right';
}

export type SortDirection = 'asc' | 'desc' | null;

export interface SortEvent {
  column: string;
  direction: SortDirection;
}

@Component({
  selector: 'ds-table',
  standalone: true,
  templateUrl: './table.component.html',
  styleUrl: './table.component.css',
  host: {
    'class': 'ds-table-wrapper'
  }
})
export class TableComponent<T extends Record<string, any>> {
  // Inputs
  readonly columns = input.required<TableColumn[]>();
  readonly data = input.required<T[]>();
  readonly striped = input(false);
  readonly hoverable = input(true);
  readonly bordered = input(false);
  readonly compact = input(false);
  readonly loading = input(false);
  readonly emptyMessage = input('No hay datos disponibles');
  readonly selectable = input(false);

  // Internal state
  readonly sortColumn = signal<string | null>(null);
  readonly sortDirection = signal<SortDirection>(null);
  readonly selectedRows = signal<Set<number>>(new Set());

  // Outputs
  readonly sort = output<SortEvent>();
  readonly rowClick = output<T>();
  readonly selectionChange = output<T[]>();

  // Computed
  readonly tableClasses = computed(() => {
    const classes = ['ds-table'];
    if (this.striped()) classes.push('ds-table--striped');
    if (this.hoverable()) classes.push('ds-table--hoverable');
    if (this.bordered()) classes.push('ds-table--bordered');
    if (this.compact()) classes.push('ds-table--compact');
    return classes.join(' ');
  });

  readonly allSelected = computed(() => {
    const data = this.data();
    return data.length > 0 && this.selectedRows().size === data.length;
  });

  readonly someSelected = computed(() => {
    const selected = this.selectedRows();
    return selected.size > 0 && selected.size < this.data().length;
  });

  onSort(column: TableColumn): void {
    if (!column.sortable) return;

    let direction: SortDirection = 'asc';

    if (this.sortColumn() === column.key) {
      if (this.sortDirection() === 'asc') {
        direction = 'desc';
      } else if (this.sortDirection() === 'desc') {
        direction = null;
      }
    }

    this.sortColumn.set(direction ? column.key : null);
    this.sortDirection.set(direction);

    this.sort.emit({ column: column.key, direction });
  }

  onRowClick(row: T): void {
    this.rowClick.emit(row);
  }

  toggleSelectAll(): void {
    if (this.allSelected()) {
      this.selectedRows.set(new Set());
    } else {
      this.selectedRows.set(new Set(this.data().map((_, i) => i)));
    }
    this.emitSelectionChange();
  }

  toggleRowSelection(index: number): void {
    const current = new Set(this.selectedRows());
    if (current.has(index)) {
      current.delete(index);
    } else {
      current.add(index);
    }
    this.selectedRows.set(current);
    this.emitSelectionChange();
  }

  isRowSelected(index: number): boolean {
    return this.selectedRows().has(index);
  }

  private emitSelectionChange(): void {
    const selected = this.data().filter((_, i) => this.selectedRows().has(i));
    this.selectionChange.emit(selected);
  }

  getSortIcon(column: TableColumn): string {
    if (this.sortColumn() !== column.key) return 'neutral';
    return this.sortDirection() || 'neutral';
  }

  getCellValue(row: T, key: string): any {
    return row[key];
  }
}
