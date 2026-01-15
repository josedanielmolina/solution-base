import { Component, computed, input, signal, contentChildren, AfterContentInit } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { AccordionItemComponent } from './accordion-item.component';

@Component({
  selector: 'ds-accordion',
  standalone: true,
  imports: [NgTemplateOutlet],
  templateUrl: './accordion.component.html',
  styleUrl: './accordion.component.css',
  host: {
    'class': 'ds-accordion-wrapper'
  }
})
export class AccordionComponent implements AfterContentInit {
  // Inputs
  readonly multiple = input(false);
  readonly variant = input<'default' | 'bordered' | 'separated'>('default');

  // Content children
  readonly items = contentChildren(AccordionItemComponent);

  // Internal state
  readonly expandedItems = signal<Set<string>>(new Set());

  // Computed
  readonly accordionClasses = computed(() => {
    return ['ds-accordion', `ds-accordion--${this.variant()}`].join(' ');
  });

  ngAfterContentInit(): void {
    // Initialize expanded state from items
    this.items().forEach(item => {
      if (item.expanded()) {
        this.expandedItems().add(item.id());
      }
    });
  }

  toggleItem(id: string): void {
    const current = new Set(this.expandedItems());

    if (current.has(id)) {
      current.delete(id);
    } else {
      if (!this.multiple()) {
        current.clear();
      }
      current.add(id);
    }

    this.expandedItems.set(current);
  }

  isExpanded(id: string): boolean {
    return this.expandedItems().has(id);
  }
}
