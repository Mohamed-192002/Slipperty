import {Component, Input, OnInit} from '@angular/core';
import {LoadingSpinner, NotFounded} from "../../Helper/html-helpers";
import {BaseComponent} from "../../base/base.component";
import {OrderService} from "../../Services/order.service";
import {ToasterAlertsService} from "../../Helper/ToasterAlert";

@Component({
  selector: 'app-user-orders',
  templateUrl: './user-orders.component.html',
  styleUrls: ['./user-orders.component.css']
})
export class UserOrdersComponent extends  BaseComponent implements  OnInit{
 @Input() OrderId : number = -1;
  protected readonly NotFounded = NotFounded;
  Loading : boolean = true;
  constructor(private orderSerivce : OrderService, private alert : ToasterAlertsService) {
    super();
  }

  ngOnInit(): void {
        this.getOrders();
    }




  orders : any [] = [];

  getOrders(){
    this.Loading = true;
    this.orderSerivce.getUserOrders(this.OrderId).subscribe(res => {
      this.orders = res;
      this.Loading = false;
    }, error => {
        this.alert.error();
        this.Loading = false;
      })
  }


  paginate(event: any){
    console.log(event);
  }

  protected readonly LoadingSpinner = LoadingSpinner;
}
