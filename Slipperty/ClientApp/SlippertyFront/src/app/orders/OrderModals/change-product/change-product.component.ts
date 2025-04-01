import {AfterViewInit, Component, Input, OnInit} from '@angular/core';
import {OrderService} from "../../../Services/order.service";
import {ApiService} from "../../../Services/api.service";
import {LoadingSpinner, NotFounded} from "../../../Helper/html-helpers";
import {AppConfigService} from "../../../Services/app-config.service";
import {ProductService} from "../../../Services/product.service";
import {ToasterAlertsService} from "../../../Helper/ToasterAlert";
import {BaseComponent} from "../../../base/base.component";

@Component({
  selector: 'app-change-product',
  templateUrl: './change-product.component.html',
  styleUrls: ['./change-product.component.css']
})
export class ChangeProductComponent extends  BaseComponent  implements OnInit {
  @Input() OrderId : number | null = null;
  @Input() OrderDetailId : number | null = null;
  @Input() CallBackFunction : Function = () => {};
  @Input() close : Function = () => {};
  products : any [] = [];
  searchQuery: string = '';
  product : any;
  selectProductId : number | null = null;
  spinner = LoadingSpinner;
  Loading = true;

  baseUrl : string = AppConfigService.config.apiUrl;
  constructor(private orderService : OrderService , private productService : ProductService, private alert : ToasterAlertsService){
    super();
  }

  ngOnInit() : void {
    console.log("From Add Product Popu" , this.OrderId);
    this.getProducts();
  }

  getProducts() : any{
    this.Loading = true;
    this.productService.GetProducts(this.page).subscribe({
      next : (data : any)=>{
        this.products = data;
        this.product = [];
        // console.log("Products  " , data);
        this.Loading = false;
      },
      error : (error : any)=>{
        this.alert.error();
        this.Loading = false;
      }
    })
  }

  get filteredProducts(): any[] {
    return this.products.filter((product) =>
      product.arbName.toLowerCase().includes(this.searchQuery.toLowerCase())
    );
  }

  openProduct(id: number, orderDetailId: number) {
    this.selectProductId = id;
  }

  ChangeOrderProduct(data : any){
    if(this.OrderDetailId != null)
    this.orderService.ChangeOrderProduct(this.OrderDetailId , data).subscribe((res : any) => {
      this.alert.success();
      this.CallBackFunction();
      this.close() ;
    }, (error: any) => {
      this.alert.error();
    })
  }



  protected readonly NotFounded = NotFounded;
  protected readonly console = console;
  protected readonly LoadingSpinner = LoadingSpinner;

}
