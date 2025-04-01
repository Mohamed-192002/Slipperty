import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderModifyComponentComponent } from './order-modify-component.component';

describe('OrderModifyComponentComponent', () => {
  let component: OrderModifyComponentComponent;
  let fixture: ComponentFixture<OrderModifyComponentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OrderModifyComponentComponent]
    });
    fixture = TestBed.createComponent(OrderModifyComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
