import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModifyOrderDetailModalComponent } from './modify-order-detail-modal.component';

describe('ModifyOrderDetailModalComponent', () => {
  let component: ModifyOrderDetailModalComponent;
  let fixture: ComponentFixture<ModifyOrderDetailModalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ModifyOrderDetailModalComponent]
    });
    fixture = TestBed.createComponent(ModifyOrderDetailModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
