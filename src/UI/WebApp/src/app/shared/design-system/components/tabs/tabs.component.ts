import { Component, computed, input, output, signal } from '@angular/core';

@Component({
  selector: 'ds-tabs',
  standalone: true,
  templateUrl: './tabs.component.html',
  styleUrl: './tabs.component.css',
  host: {
    'class': 'ds-tabs-wrapper'
  }
})
export class TabsComponent {
  // Inputs
  readonly tabs = input.required<TabItem[]>();
  readonly activeTab = input<string | undefined>(undefined);
  readonly variant = input<'underline' | 'pills' | 'boxed'>('underline');
  readonly size = input<'sm' | 'md' | 'lg'>('md');
  readonly fullWidth = input(false);

  // Internal state
  private readonly internalActiveTab = signal<string | null>(null);

  // Outputs
  readonly tabChange = output<string>();

  // Computed
  readonly currentTab = computed(() => {
    return this.activeTab() || this.internalActiveTab() || this.tabs()[0]?.id;
  });

  readonly tabsClasses = computed(() => {
    const classes = [
      'ds-tabs',
      `ds-tabs--${this.variant()}`,
      `ds-tabs--${this.size()}`
    ];
    if (this.fullWidth()) classes.push('ds-tabs--full-width');
    return classes.join(' ');
  });

  selectTab(tabId: string): void {
    const tab = this.tabs().find(t => t.id === tabId);
    if (tab && !tab.disabled) {
      this.internalActiveTab.set(tabId);
      this.tabChange.emit(tabId);
    }
  }

  getTabClasses(tab: TabItem): string {
    const classes = ['ds-tab'];
    if (this.currentTab() === tab.id) classes.push('ds-tab--active');
    if (tab.disabled) classes.push('ds-tab--disabled');
    return classes.join(' ');
  }
}

export interface TabItem {
  id: string;
  label: string;
  disabled?: boolean;
  icon?: string;
  badge?: string | number;
}
