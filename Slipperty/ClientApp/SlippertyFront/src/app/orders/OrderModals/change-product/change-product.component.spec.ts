import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeProductComponent } from './change-product.component';

describe('ChangeProductComponent', () => {
  let component: ChangeProductComponent;
  let fixture: ComponentFixture<ChangeProductComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ChangeProductComponent]
    });
    fixture = TestBed.createComponent(ChangeProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
