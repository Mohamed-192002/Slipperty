import {Component, Input, OnInit} from '@angular/core';
import {LoadingSpinner, NotFounded} from "../../../Helper/html-helpers";
import {BaseComponent} from "../../../base/base.component";
import {ProductService} from "../../../Services/product.service";
import {ToasterAlertsService} from "../../../Helper/ToasterAlert";
import {AppConfigService} from "../../../Services/app-config.service";
import {AppConfig} from "../../../Entities/AppConfig";
import {OrderService} from "../../../Services/order.service";

@Component({
  selector: 'app-add-order-detail-product',
  templateUrl: './add-order-detail-product.component.html',
  styleUrls: ['./add-order-detail-product.component.css']
})
export class AddOrderDetailProductComponent extends BaseComponent implements OnInit {
  @Input() OrderId : number | null = null;
  @Input() CallBackFunction : Function = () => {};
  @Input() close : Function = () => {};
  products : any [] = [];
  searchQuery: string = '';
  selectProductId : number | null = null;
  spinner : string  = LoadingSpinner;
  Loading : boolean = true;

  baseUrl : string = AppConfigService.config.apiUrl;
  constructor(private orderService : OrderService , private productService : ProductService, private alert : ToasterAlertsService){
    super();
  }

  ngOnInit() : void {
    this.getProducts();
  }

  getProducts() : any{
      this.Loading = true;
      this.productService.GetProducts(this.page).subscribe({
        next : (data : any)=>{
          this.products = data;
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

  AddOrder(data:any){
      if(this.OrderId != null)
      this.orderService.AddOrderProduct(this.OrderId,data).subscribe(res => {
        this.alert.success();
        this.CallBackFunction();
        this.close();
      },err => this.alert.error())
  }
  protected readonly NotFounded = NotFounded;
  protected readonly console = console;
  protected readonly LoadingSpinner = LoadingSpinner;
}
