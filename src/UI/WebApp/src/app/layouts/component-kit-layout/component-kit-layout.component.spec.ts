import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ComponentKitLayoutComponent } from './component-kit-layout.component';

describe('ComponentKitLayoutComponent', () => {
  let component: ComponentKitLayoutComponent;
  let fixture: ComponentFixture<ComponentKitLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComponentKitLayoutComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(ComponentKitLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
