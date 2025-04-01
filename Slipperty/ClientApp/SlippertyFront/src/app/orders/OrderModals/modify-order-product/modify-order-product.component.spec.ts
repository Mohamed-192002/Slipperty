import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModifyOrderProductComponent } from './modify-order-product.component';

describe('ModifyOrderProductComponent', () => {
  let component: ModifyOrderProductComponent;
  let fixture: ComponentFixture<ModifyOrderProductComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ModifyOrderProductComponent]
    });
    fixture = TestBed.createComponent(ModifyOrderProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
