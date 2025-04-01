import {Component, EventEmitter, Input, OnChanges, Output, SimpleChanges} from '@angular/core';
import {ProductService} from "../../../Services/product.service";
import {ToasterAlertsService} from "../../../Helper/ToasterAlert";
import {AppConfigService} from "../../../Services/app-config.service";

@Component({
  selector: 'app-order-detail-product',
  templateUrl: './order-detail-product.component.html',
  styleUrls: ['./order-detail-product.component.css']
})
export class OrderDetailProductComponent implements OnChanges {
  product: any;
  @Input() selectProductId: number | null = -1;
  @Output() selectProductIdChange = new EventEmitter<number | null>();
  @Output() GetOrderProductData = new EventEmitter<any | null>();

  baseUrl : string = AppConfigService.config.apiUrl;
  ngOnChanges(changes: SimpleChanges): void {
    if (changes["selectProductId"] && changes["selectProductId"].currentValue !== -1) {
      this.productService.GetProductById(changes["selectProductId"].currentValue).subscribe({
        next: result => {
          this.product = result;
        },
        error: err => {
          this.alert.error();
        }
      });
    }
  }
  constructor(public productService: ProductService, private  alert : ToasterAlertsService) {
  }


  selectedVariable: { [key: string]: string } = {};
  quantity: number = 1;
  setCombinationValue(variableName: string, value: string) {
    console.log("set combination . ");
    this.selectedVariable[variableName] = value;
    console.log(this.selectedVariable);
  }

  decreaseQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  increaseQuantity() {
    this.quantity++;
  }

  OrderProductData() {
    let combinationId = this.isCombinationValid();
    if(combinationId == -1){
      this.alert.error("غير متوفر فى المخزن");
      return;
    }
    let data =  {
        //selectedVariable: this.selectedVariable,
        productId : this.product.id,
        quantity: this.quantity,
        combinationId : combinationId
      };
    this.GetOrderProductData.emit(data);
  }

  isCombinationValid(): number {
    let combinationId: number = -1;
    const selectedValues: string[] = Object.values(this.selectedVariable);
    console.log(this.selectedVariable)
    console.log(this.product.variableCombinations);
    for (const combination of this.product.variableCombinations) {
      const combinationValues = combination.text.split(",");
      const isMatching = selectedValues.length === combinationValues.length &&
        selectedValues.every(value => combination.text.includes(value));

      if (isMatching) {
        if (combination.stockCount >= this.quantity) {
          return combination.id;
        } else {
          this.alert.error("يجب ان يكون عدد القطع المطلوب اقل من او يساوي القطع فى المخزن");
          return -1;
        }
      }
    }

    return combinationId;
  }


  back(){
    this.selectProductIdChange.emit(null);
  }
}
