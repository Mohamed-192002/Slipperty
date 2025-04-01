import { Component, Input } from '@angular/core';
import {OrderService} from "../../../Services/order.service";
import {ToasterAlertsService} from "../../../Helper/ToasterAlert";

@Component({
  selector: 'app-order-modify-component',
  templateUrl: './order-modify-component.component.html',
  styleUrls: ['./order-modify-component.component.css']
})
export class OrderModifyComponentComponent {
  @Input() orderId!: number;
  @Input() combinationId?: number;
  quantity: number = 1;
  combinationText: string = '';
  constructor(private orderService: OrderService , private  alert : ToasterAlertsService) {}

  setCombinationValue(clickedButton: HTMLElement) {
    const parentDiv = clickedButton.closest('.combinationValue') as HTMLElement;
    const buttons = parentDiv.querySelectorAll('button');
    buttons.forEach(button => {
      button.classList.remove('btn-primary', 'text-white', 'selected-value');
    });

    clickedButton.classList.add('btn-primary', 'text-white', 'selected-value');
  }

  increaseQuantity() {
    this.quantity += 1;
  }

  decreaseQuantity() {
    if (this.quantity > 1) {
      this.quantity -= 1;
    }
  }

  orderModifiedProduct(modalId: string) {
    let combinationTextArray: string[] = [];
    document.querySelectorAll(`#${modalId} button.selected-value`).forEach((button: any) => {
      const varailbeName = button.getAttribute('data-varailbename');
      const value = button.innerHTML.trim();
      combinationTextArray.push(`${varailbeName}: ${value}`);
    });

    this.combinationText = combinationTextArray.join(',');

    const requestData = {
      Text: this.combinationText,
      Qty: this.quantity,
      Id: this.orderId,
      CombinationId: this.combinationId
    };

    this.performOrderAction(requestData, modalId);
  }

  performOrderAction(requestData: any, modalId: string) {
    this.orderService.modifyOrderDetail(this.orderId, requestData).subscribe(
      {
        next : () => {
          this.alert.success();
          this.closeModal(modalId);
        },
        error: () => {
          this.alert.error();
        }
      }
    );
  }



  closeModal(modalId: string) {
    const modal = document.getElementById(modalId);
    if (modal) {
      modal.classList.remove('show');
      modal.setAttribute('aria-hidden', 'true');
      modal.style.display = 'none';
    }
  }



}
