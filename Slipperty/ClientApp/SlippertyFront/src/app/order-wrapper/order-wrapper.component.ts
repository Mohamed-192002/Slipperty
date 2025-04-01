import {Component, OnInit, OnDestroy, Renderer2, ElementRef, AfterViewInit} from '@angular/core';
import {OrderTransactionNotificationService} from "../Services/order-transaction-notification.service";
import {OrderService} from "../Services/order.service";

export class OrderSummary {
  newOrdersCount: number;          // اوردرات جديدة
  attemptedOrdersCount: number;    // تم المحاولة
  whatsappOrdersCount: number;     // واتساب
  holdOrdersCount: number;         // تأجيلات
  recallOrdersCount: number;       // اعادة اتصال
  stoppedOrdersCount: number;      // وقف التشغيل
  noAttemptYesterdayCount: number; // لم يتم المحاولة امس
  confirmedOrdersCount: number;    // طلبات تم التأكيد
  cancel : number;
  triedOrderCount : number;
  constructor() {
    this.newOrdersCount = 0;
    this.attemptedOrdersCount = 0;
    this.whatsappOrdersCount = 0;
    this.holdOrdersCount = 0;
    this.recallOrdersCount = 0;
    this.stoppedOrdersCount = 0;
    this.noAttemptYesterdayCount = 0;
    this.confirmedOrdersCount = 0;
    this.cancel = 0;
    this.triedOrderCount = 0;
  }
}


@Component({
  selector: 'app-order-wrapper',
  templateUrl: './order-wrapper.component.html',
  styleUrls: ['./order-wrapper.component.css']
})
export class OrderWrapperComponent implements OnInit , OnDestroy {
  orderCounters: OrderSummary = new OrderSummary();

  constructor(private orderService: OrderService , private orderTransactionNotifcationService : OrderTransactionNotificationService,private el: ElementRef, private renderer: Renderer2) {}



  ngOnInit(): void {
       this.orderService.getOrderSummary().subscribe(orderSummary => {
         // console.log(orderSummary);
         this.orderCounters = orderSummary as OrderSummary;
         this.orderTransactionNotifcationService.ReceiveOrderTransaction(this.updateOrderCounter.bind(this));
       })
  }
  updateOrderCounter(response : any){
    this.orderCounters = response as OrderSummary;
  }

  ngOnDestroy(): void {
    this.orderTransactionNotifcationService.DestroyHubConnection();
  }


}
