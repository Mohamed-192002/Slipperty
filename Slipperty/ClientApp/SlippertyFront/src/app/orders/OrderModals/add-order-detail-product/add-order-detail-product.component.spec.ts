import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddOrderDetailProductComponent } from './add-order-detail-product.component';

describe('AddOrderDetailProductComponent', () => {
  let component: AddOrderDetailProductComponent;
  let fixture: ComponentFixture<AddOrderDetailProductComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddOrderDetailProductComponent]
    });
    fixture = TestBed.createComponent(AddOrderDetailProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
