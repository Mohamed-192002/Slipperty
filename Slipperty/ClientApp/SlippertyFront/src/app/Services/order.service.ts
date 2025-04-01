import {ApplicationRef, ComponentFactoryResolver, Injectable, Injector, ViewContainerRef} from '@angular/core';
import {ApiService} from "./api.service";
import {ActionType} from "../Entities/ActionType";
import {Observable, throwError} from "rxjs";
import {ToasterAlertsService} from "../Helper/ToasterAlert";
import {ChangeProductComponent} from "../orders/OrderModals/change-product/change-product.component";
import {ModalService} from "./modal.service";
import {ModifyOrderProductComponent} from "../orders/OrderModals/modify-order-product/modify-order-product.component";

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private modalContainer!: ViewContainerRef;

  constructor(private  api : ApiService, private  alert : ToasterAlertsService, private resolver: ComponentFactoryResolver, private injector: Injector,private appRef: ApplicationRef, private modalService : ModalService) { }



  public getProductId(OrderDetailId : number)
  {
    return this.api.Get(`GetProductId/${OrderDetailId}`);
  }
  public AddOrderProduct(orderId : number , data : any){
    return this.api.Post(`AddOrderProduct/${orderId}`, data);
  }
  public ChangeOrderProduct(orderDetailId : number  , data : any){
    return this.api.Post(`ChangeOrderProduct/${orderDetailId}`, data);
  }
  public getUserOrders(orderId : number){
  return this.api.Get(`GetUserOrders/${orderId}`);
  }
  setModalContainer(viewContainerRef: ViewContainerRef) {
    this.modalContainer = viewContainerRef;
  }
  public getOrderSummary(){
    return this.api.Get("GetOrderSummary");
  }
  public getOrderDetail(orderId: number, status: string, actionStatus : number = -1, data : any) {
    let url : string = "";
    if(data != null){
      let filterData : string = ""
        Object.keys(data).forEach( _ => {
        filterData += `&${_}=${data[_]}`;
      })
      console.log(url);
       url = `Admin/Orders/GetOrderCardDetail?OrderId=${orderId}&status=${status}&ActionStatus=${actionStatus}${filterData}`;

    }else {
      url = `Admin/Orders/GetOrderCardDetail?OrderId=${orderId}&status=${status}&ActionStatus=${actionStatus}`;
    }
    return this.api.Get(url, { responseType: 'text' });
  }
  public GetOrders(status: number, page?: number, queryString?: string) {
    let url = `Admin/Orders/GetOrders?status=${status}`;
    if (page !== null && page !== undefined) {
      url += `&page=${page}`;
    }
    if (queryString) {
      url += `&${queryString}`;
    }

    return this.api.Get(url, { responseType: 'text' });
  }
  public sendOrderAction(orderId: number, actionType: number, requestData: any) {
    return this.api.Post(`OrderAction/${orderId}/${actionType}`, requestData);
  }


  public DeleteOrderDetailProduct(orderDetailId : number) : Observable<any>{
    return this.api.delete(`DeleteOrderDetailProduct/${orderDetailId}`);
  }
  public loadTabContent(){
    return this.api.Get(`GetOrders`, { responseType: 'text' });
  }

  public getActionData(actionType: ActionType, modalId: string): any {
    switch (actionType) {
      case ActionType.Confirmed:
      case ActionType.StoppedConfirmed:
        return {};
      case ActionType.FollowUps:
        return this.getFollowUpData(modalId);
      case ActionType.Cancel:
        return this.getCancelData(modalId);
      case ActionType.Holding:
        return this.getPostponementData(modalId);
      case ActionType.Urgent:
        return this.getUrgentData(modalId);
      case ActionType.Modified:
        return this.getOrderModificationData(modalId);
      case ActionType.ModificationDeclined:
        return {};
      default:
        console.error('Action type not implemented');
        return {};
    }
  }

  public GetOrderById(OrderId : number) : Observable<any>{
    return this.api.Get(`GetOrderById/${OrderId}`);
  }



  private getUrgentData(modalId: string) {
    let inputs = document.querySelectorAll(`#${modalId} li input`);
    return {
      UrgentDate: (inputs[0] as HTMLInputElement)?.value,
      UrgentAdditionalPrice: (inputs[1] as HTMLInputElement)?.value
    };
  }

  private getPostponementData(modalId: string) {
    let inputs = document.querySelectorAll(`#${modalId} li input`);
    return {
      PostponementDate: (inputs[0] as HTMLInputElement)?.value,
      ShipmentPrice: (inputs[1] as HTMLInputElement)?.value
    };
  }

  private getCancelData(modalId: string) {
    let activeItem = document.querySelector(`#${modalId} li.active`);
    if (!activeItem) return {};
    return {
      Notes: (activeItem.querySelector('input') as HTMLInputElement)?.value,
      Id: Array.from(activeItem.parentElement!.children).indexOf(activeItem) + 1
    };
  }

  private getFollowUpData(modalId: string) {
    // let activeItemSend = document.querySelector(`#${modalId} li.active`);
    // if (!activeItemSend) return {};
    // let activeItemIndex = Array.from(activeItemSend.parentElement!.children).indexOf(activeItemSend);
    if (modalId.startsWith('*')) {
      return {
        ContactMethodId: 2,
        ContactStatusId: 5,
        FollowUpNote: ''
      };
    }
    let activeItem = document.querySelector(`#${modalId} li.active`);
    if (!activeItem) return {};
    return {
      Notes: (activeItem.querySelector('input') as HTMLInputElement)?.value,
      ContactStatusId: Array.from(activeItem.parentElement!.children).indexOf(activeItem) + 1,
    };
  }

  private getOrderModificationData(modalId: string) {
    let modifiedObj: any = {};
    document.querySelectorAll(`#${modalId} li [name]`).forEach(element => {
      modifiedObj[(element as HTMLInputElement).name] = (element as HTMLInputElement).value;
    });
    return modifiedObj;
  }

  public openOrderDetailModifiedProductModal(actionUrl: string, callBackFunction : Function = () => {}, OrderId : number | null  = null): void {
    document.getElementById('loader')?.classList.remove('d-none');
    document.getElementById('ajaxModal')!.innerHTML = '';
    var orderDetailId = Number(actionUrl.split("&")[1].split("=")[1]);
    console.log("Order Detail Id" , orderDetailId)
    this.api.Get(actionUrl, { responseType: 'text' }).subscribe({
      next:(data) => {
        if(actionUrl.split("&")[0] == "Admin/Orders/GetOrdersDetailProduct?ActionMod=Change"){
          // console.log("url Action After Split " , actionUrl.split("&")[0])
          // const factory = this.resolver.resolveComponentFactory(ChangeProductComponent);
          // const componentRef = factory.create(this.injector);
          // this.appRef.attachView(componentRef.hostView);
          // document.getElementById('ajaxModal')?.appendChild(componentRef.location.nativeElement);
          // componentRef.instance.Products = data;
          // var orderDetailId = Number(actionUrl.split("&")[1].split("=")[1]);
   console.log("Order Detail Id" , orderDetailId);
          this.modalService.open(ChangeProductComponent, {OrderId : OrderId,OrderDetailId : orderDetailId , CallBackFunction : callBackFunction});
          document.getElementById('loader')?.classList.add('d-none');
          return;

        }else if(actionUrl.split("&")[0] == "Admin/Orders/GetOrdersDetailProduct?ActionMod=Edit"){

           document.getElementById('ajaxModal')!.innerHTML = data;
           this.setUpModifyProductModal(orderDetailId , callBackFunction);
         }else{
           document.getElementById('ajaxModal')!.innerHTML = data;
           this.setUpDeleteProductModal(callBackFunction);

        }
        // // @ts-ignore
        // ($('#OrderDetailModifyProductModal') as any).modal('show');

        // @ts-ignore
        ($('#DeleteOrderDetailConfirmation') as any).modal('show');
        document.getElementById('loader')?.classList.add('d-none');
      },
    error:(error) => {
        console.error('Error:', error);
        document.getElementById('ajaxModal')!.innerHTML = '';
        document.getElementById('loader')?.classList.add('d-none');
      }
    });
  }

  private setUpDeleteProductModal(callBackFunction : Function = () => {}){
    document.querySelectorAll("#DeleteOrderDetailConfirmation .order-btn").forEach(button => {
        button.addEventListener("click", (event: Event) => {
        const target = event.target as HTMLElement;
        const actionType = Number(target?.getAttribute("actionType"));
        if( !isNaN(actionType)  && actionType == ActionType.DeleteOrderDetail){
          let orderDeleteId = Number((document.getElementById("DeleteOrderDetailConfirmation") as HTMLElement).getAttribute("data-Id"));
          if(!isNaN(orderDeleteId))
             this.DeleteOrderDetailProduct(orderDeleteId).subscribe({
               next : (res:any)=>{
                 this.alert.success();
                 callBackFunction();
                 //@ts-ignore
                 $("#DeleteOrderDetailConfirmation").modal('hide');
               },
               error:(err:any)=>{
                   this.alert.error();
               }
             });
          else{
            this.alert.error();
          }
        }
        else if(!isNaN(actionType) && actionType == ActionType.CloseModal){
          //@ts-ignore
          $("#DeleteOrderDetailConfirmation").modal('hide');
        }
      });
    });
  }
  private setUpModifyProductModal(orderDetailId : number, callBackFunction : Function = () => {}){
    // (window as any).setCombinationValue = (clickedButton: HTMLElement) => {
    //   const combinationValueDiv = clickedButton.closest('.combinationValue') as HTMLElement;
    //   const buttons = combinationValueDiv.querySelectorAll('button');
    //   buttons.forEach(button => {
    //     button.classList.remove('btn-primary', 'text-white', 'selected-value');
    //   });
    //   clickedButton.classList.add('btn-primary', 'text-white', 'selected-value');
    // };
    //
    // (window as any).increaseQuantity = (button: HTMLElement) => {
    //   const input = button.previousElementSibling as HTMLInputElement;
    //   const currentValue = parseInt(input.value, 10);
    //   input.value = (currentValue + 1).toString();
    // };
    //
    // (window as any).decreaseQuantity = (button: HTMLElement) => {
    //   const input = button.nextElementSibling as HTMLInputElement;
    //   const currentValue = parseInt(input.value, 10);
    //   if (currentValue > 1) {
    //     input.value = (currentValue - 1).toString();
    //   }
    // };
    //
    // (window as any).OrderModifiedProduct = (modalDataContainerId: string) => {
    //   let combinationText = "";
    //   document.querySelectorAll(`#${modalDataContainerId} button.selected-value`).forEach((val: any) => {
    //     const varailbeName = val.getAttribute("data-varailbename");
    //     combinationText += `${varailbeName}:${val.innerHTML.trim()},`;
    //   });
    //
    //   if (combinationText.endsWith(",")) {
    //     combinationText = combinationText.slice(0, -1);
    //   }
    //   // @ts-ignore
    //   let id = $("#OrderDetailModifyProductModal").data("id");
    //   // @ts-ignore
    //   let CombinationId = $("#OrderDetailModifyProductModal").data("combinationid");
    //   const obj = {
    //     Text: combinationText,
    //     Qty: (document.getElementById("quantityInput") as HTMLInputElement).value,
    //     Id: id,
    //     CombinationId: CombinationId
    //   };
    //    console.log(obj);
    //   (window as any).performOrderAction(obj, 'OrderDetailModifyProductModal', callBackFunction);
    // };

   this.modalService.open(ModifyOrderProductComponent, {OrderDetailId : orderDetailId , CallBackFunction : callBackFunction});
    //
    // (window as any).performOrderAction = (requestData: any, modalId: string, loadCallBackFunction: () => void = () => {}) => {
    //   let orderId =  Number((document.querySelector('.order-card') as HTMLElement).getAttribute('order-id'));
    //
    //   this.api.Post(`ModifiyOrderDetailProduct/${orderId}`, requestData).subscribe(
    //     () => {
    //       this.alert.success();
    //       // @ts-ignore
    //       $("#OrderDetailModifyProductModal").modal("hide");
    //
    //       loadCallBackFunction();
    //     },
    //     () => {
    //       this.alert.error();
    //     }
    //   );
    // };


  }


  modifyOrderDetail(orderDetailId: number, requestData: any): Observable<any> {
    return this.api.Post(`ModifiyOrderDetailProduct/${orderDetailId}`, requestData);
  }
  private handleError(error: any): Observable<never> {
    console.error('Error occurred:', error);
    return throwError(() => new Error('Something went wrong; please try again later.'));
  }


}
