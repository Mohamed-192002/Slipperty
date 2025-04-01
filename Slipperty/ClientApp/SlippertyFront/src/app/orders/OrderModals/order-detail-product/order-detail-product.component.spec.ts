import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderDetailProductComponent } from './order-detail-product.component';

describe('OrderDetailProductComponent', () => {
  let component: OrderDetailProductComponent;
  let fixture: ComponentFixture<OrderDetailProductComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OrderDetailProductComponent]
    });
    fixture = TestBed.createComponent(OrderDetailProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
