import {Component, Input, OnInit} from '@angular/core';
import { OrderService } from 'src/app/Services/order.service';
import {ToasterAlertsService} from "../../../Helper/ToasterAlert";

@Component({
  selector: 'app-modify-order-product',
  templateUrl: './modify-order-product.component.html',
  styleUrls: ['./modify-order-product.component.css']
})
export class ModifyOrderProductComponent implements OnInit{
   productId: number | null  = null;
  @Input() OrderDetailId: number | null = null;
  @Input() CallBackFunction : Function = () => {};
  @Input() close : Function = () => {};
  constructor(private orderService : OrderService, private  alert : ToasterAlertsService) {
  }

  ngOnInit(): void {
    if(this.OrderDetailId != null)
       this.orderService.getProductId(this.OrderDetailId).subscribe((productId : any ) => {
         this.productId = productId;
       }, (err : any) => {
          this.alert.error();
       })
  }

  UpdateOrderProduct(data : any){

    this.orderService.modifyOrderDetail(this.OrderDetailId??-1 , data).subscribe({
      next : (res : any) => {
        this.alert.success();
        this.CallBackFunction();
        this.close();
      },
      error : (err : any) => {
      }
    })
  }
}
