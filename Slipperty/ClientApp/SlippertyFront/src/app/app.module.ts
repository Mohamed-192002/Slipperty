import {NgModule, Injector, ApplicationRef, ComponentFactoryResolver, APP_INITIALIZER} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import {HttpClientModule} from "@angular/common/http";
import { OrderWrapperComponent } from './order-wrapper/order-wrapper.component';
import { OrdersComponent } from './orders/orders.component';
import {AppConfigService} from "./Services/app-config.service";
import { OrderModifyComponentComponent } from './orders/OrderModals/order-modify-component/order-modify-component.component';
import { GeneralNotificationComponent } from './general-notification/general-notification.component';
import { ChangeProductComponent } from './orders/OrderModals/change-product/change-product.component';
import { OrderCardDetailComponent } from './orders/order-card-detail/order-card-detail.component';
import { ModalContainerComponent } from './shared/modal-container/modal-container.component';
import {
  AddOrderDetailProductComponent
} from "./orders/OrderModals/add-order-detail-product/add-order-detail-product.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {OrderDetailProductComponent} from "./orders/OrderModals/order-detail-product/order-detail-product.component";
import { PaginationComponent } from './shared/pagination/pagination.component';
import {
  ModifyOrderDetailModalComponent
} from "./orders/OrderModals/modify-order-detail-modal/modify-order-detail-modal.component";
import { TimeInputComponent } from './shared/time-input/time-input.component';
import {UserOrdersComponent} from "./User/user-orders/user-orders.component";
import { ModifyOrderProductComponent } from './orders/OrderModals/modify-order-product/modify-order-product.component';
import { ConfirmationComponent } from './Helper/confirmation/confirmation.component';


export function initializeApp(appConfigService: AppConfigService) {
  return () => appConfigService.loadConfig();
}


const componentMap: Record<string, any> = {
  'app-root': AppComponent,
  'app-order-wrapper' : OrderWrapperComponent,
  'app-orders': OrdersComponent,
  'app-general-notification' : GeneralNotificationComponent,
};

@NgModule({
  declarations: [AppComponent,  OrderWrapperComponent, OrdersComponent, OrderModifyComponentComponent, GeneralNotificationComponent, ChangeProductComponent, OrderCardDetailComponent, ModalContainerComponent, AddOrderDetailProductComponent, OrderDetailProductComponent, PaginationComponent, ModifyOrderDetailModalComponent, TimeInputComponent, UserOrdersComponent, ModifyOrderProductComponent, ConfirmationComponent],
  imports: [BrowserModule,HttpClientModule , ReactiveFormsModule , FormsModule ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [AppConfigService],
      multi: true
    }
  ],
  bootstrap: [],
})
export class AppModule {
  constructor(private injector: Injector, private appRef: ApplicationRef, private resolver: ComponentFactoryResolver) {}

  ngDoBootstrap() {
    for (const selector in componentMap) {
      if (document.querySelector(selector)) {
        this.renderComponent(selector);
      }
    }
  }

  private renderComponent(selector: string) {
    const component = componentMap[selector];
    if (!component) {
      console.warn(`⚠️ No Angular component found for selector: ${selector}`);
      return;
    }

    const factory = this.resolver.resolveComponentFactory(component);
    const componentRef = factory.create(this.injector);

    this.appRef.attachView(componentRef.hostView);
    document.querySelector(selector)?.appendChild(componentRef.location.nativeElement);

    //console.log(`✅ Bootstrapped component: ${selector}`);
  }
}
