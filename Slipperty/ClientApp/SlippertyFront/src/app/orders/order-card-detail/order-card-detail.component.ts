import {AfterViewInit, Component, HostListener, Input, OnInit, Renderer2} from '@angular/core';
import {ErrorDuringLoadingData, LoadingSpinner} from 'src/app/Helper/html-helpers';
import {OrderService} from "../../Services/order.service";
import {ToasterAlertsService} from "../../Helper/ToasterAlert";
import {ActionType} from "../../Entities/ActionType";
import {ModalService} from "../../Services/modal.service";
import {
  AddOrderDetailProductComponent
} from "../OrderModals/add-order-detail-product/add-order-detail-product.component";
import {
  ModifyOrderDetailModalComponent
} from "../OrderModals/modify-order-detail-modal/modify-order-detail-modal.component";
import {UserOrdersComponent} from "../../User/user-orders/user-orders.component";
import {ConfirmationComponent} from "../../Helper/confirmation/confirmation.component";

@Component({
  selector: 'app-order-card-detail',
  templateUrl: './order-card-detail.component.html',
  styleUrls: ['./order-card-detail.component.css']
})
export class OrderCardDetailComponent implements OnInit , AfterViewInit{
  @Input() InputHtml !: string ;
  @Input() OrderId !: number;
  @Input() status : string | null = "";
  @Input() LoadTabList : Function = () => {}
  constructor(private orderService : OrderService,private renderer: Renderer2, private alert : ToasterAlertsService, private modalService : ModalService) {
  }
  ngOnInit():void{
    debugger;
    this.GetOrderCardDetail(this.OrderId , this.status == null ? "" : this.status);
  }
  ngAfterViewInit():void {
  }

  @HostListener("click" , ['$event'])
  PreventBubble(event : Event){
      console.warn("child Component . ");
      event.stopPropagation();
  }

  GetOrderCardDetail(OrderId: number, status: string, data : any = null ): void {
    const container = document.getElementById('partial-container');
    if (container) {
      container.innerHTML = LoadingSpinner;
    }
    // @ts-ignore
    const tabId = window.getTabStatus((document.querySelector('.tab-active-color') as HTMLElement).id)??-1;

    this.orderService.getOrderDetail(OrderId , status, Number(tabId) , data).subscribe({
      next: (data) => {
        if(container){
          container.innerHTML = data;
          this.OrderId = Number((document.querySelector('.order-card') as HTMLElement).getAttribute('order-id'));
          this.setUpAnchorClickListeners();
          this.setUpOrderDetailProduct();
          this.initailzeModalsJsScripts();
          this.setupPaginationBtnClickListeners();
          this.setUpAddOrderOpenModalButton();
          this.setUpCopyToClibBoard();
          this.setUpUserOrdersBtn();
          this.initalizeCancelModelScripts();
          this.setUpSearchSectionToggleBtn();
          this.setUpSearchSectionBtn();
          this.setUpOrderBySectionScripts();
        }
      },
      error: (error) => {
        if(container) {
          this.LoadTabList();
          //container.innerHTML = ErrorDuringLoadingData;
        }
      }
    })
  }

  setUpUserOrdersBtn(){
      let userOrdersBtn = document.getElementById('user-orders') as HTMLButtonElement;
      if(userOrdersBtn){
         userOrdersBtn.addEventListener('click', ()=>{
             this.modalService.open(UserOrdersComponent, {OrderId : this.OrderId})
         })
      }
  }

  setUpAnchorClickListeners(): void {
    const anchors = document.querySelectorAll('a'); // Select all anchor tags
    anchors.forEach((anchor) => {
      this.renderer.listen(anchor, 'click', (event) => this.onAnchorClick(event));
    });
  }

  onAnchorClick(event: MouseEvent): void {
    const orderCard = document.querySelector('.order-card') as HTMLElement;
    if (orderCard) {
      const orderId = Number(orderCard.getAttribute('order-id'));
      const actionType = (event.currentTarget as HTMLElement).getAttribute("ActionType");
      const parsedActionType = actionType ? parseInt(actionType) : NaN;
      const modalId = (event.currentTarget as HTMLElement).getAttribute("modal-id");
      const modalDataContainerId = (event.currentTarget as HTMLElement).getAttribute("modal-data-container-id");
      if(Number.isNaN(parsedActionType)) return;
      if(parsedActionType == Number(ActionType.CloseModal)) {
        //@ts-ignore
        $(`#${modalId}`).modal('hide'); return;
      }
      if(parsedActionType == Number(ActionType.Confirmed)){
        this.modalService.open(ConfirmationComponent , {callBackFunction : () => {
            this.orderService.sendOrderAction(orderId, parsedActionType, this.orderService.getActionData(parsedActionType, modalDataContainerId??"")).subscribe({
              next: () => {
                // @ts-ignore
                $(`#${modalId}`).modal('hide');
                this.alert.success();
                // this.loadActiveTabContent();
                this.GetOrderCardDetail(orderId ,"next");
              },
              error: (err) => {
                console.error('Error:', err);
              }
            });
          }});
        return;
      }
      this.orderService.sendOrderAction(orderId, parsedActionType, this.orderService.getActionData(parsedActionType, modalDataContainerId??"")).subscribe({
        next: () => {
          // @ts-ignore
          $(`#${modalId}`).modal('hide');
          this.alert.success();
          // this.loadActiveTabContent();
          this.GetOrderCardDetail(orderId ,"next");
        },
        error: (err) => {
          console.error('Error:', err);
        }
      });
    }
  }
  setUpOrderDetailProduct(): void{
    console.log("Wellcom I am using Jquery");
    // @ts-ignore
    ($(".order-detail-product") as any).on("click" , (event) => {
      console.log("Jquery is work");
      // @ts-ignore
      this.orderService.openOrderDetailModifiedProductModal($(event.target).data('action-url'), this.currentOrder.bind(this),Number((document.querySelector('.order-card') as HTMLElement).getAttribute('order-id')));
    });
  }

  setUpAddOrderOpenModalButton(){
       (document.getElementById("add-product-icon") as HTMLElement ).addEventListener("click" , ()=>{
         const orderId = Number((document.querySelector('.order-card') as HTMLElement).getAttribute('order-id'))
         this.modalService.open(AddOrderDetailProductComponent,{OrderId : orderId, CallBackFunction : this.currentOrder.bind(this)} );
       });

  }


  setUpCopyToClibBoard(){
    (document.getElementById("copy-orderNo") as any)?.addEventListener("click", function() {
      const numberText = (document.getElementById("orderNoCopiedValue") as any)?.innerText;
      navigator.clipboard.writeText(numberText).then(() => {
        const tooltip = (document.getElementById("tooltip") as HTMLElement);
        tooltip.style.display = "inline-block";
        setTimeout(() => { tooltip.style.display = "none"; }, 1500);
      });
    });
  }
  public currentOrder(): void {
    const activeTab = document.querySelector(".tab-color.active") as HTMLElement;
    const tabId = activeTab ? Number(activeTab.id) : -1;
    const orderId = Number((document.querySelector('.order-card') as HTMLElement).getAttribute('order-id'));
    // @ts-ignore
    let status=  window?.getTabStatus(tabId);
    this.GetOrderCardDetail(orderId, "");
    // this.orderService.getOrderDetail(orderId, status).subscribe({
    //   next: (orderDetail) => {
    //     console.log(orderDetail);
    //   },
    //   error: (error) => {
    //     console.error('Error fetching order detail:', error);
    //   }
    // });
  }

  public setUpSearchSectionToggleBtn(){
    document.getElementById("searchSectionToggelBtn")?.addEventListener("click", (event : Event) => {
       var searchForm = document.getElementById("searchForm") as HTMLElement;
       console.log(searchForm);
       if(searchForm != null){
          if(searchForm.style.display == "block"){
            searchForm.style.display = "none";
            (event.target as HTMLElement).classList.remove('active');
          }else {
            searchForm.style.display = "block";
            (event.target as HTMLElement).classList.add('active');
          }

       }
    })
  }

  public setUpSearchSectionBtn(){
    document.getElementById("searchFormBtn")?.addEventListener("click", (event : Event) => {
      const orderId = Number((document.querySelector('.order-card') as HTMLElement).getAttribute('order-id'));
      this.GetOrderCardDetail(orderId,"", this.getSearchFormData())
    });
  }


  initailzeModalsJsScripts(){
    // @ts-ignore
    $('#datepicker').datepicker();

    // @ts-ignore
    $(".date-option").on("click", function () {
      // @ts-ignore
      let daysToAdd = $(this).data("days");
      let currentDate = new Date();
      currentDate.setDate(currentDate.getDate() + (daysToAdd ) );
      const formattedDate = currentDate
        .toISOString()
        .split('T')[0]
        .split('-')
        .reverse()
        .join('/');
      // @ts-ignore
      $("#datepicker").val(formattedDate);
    });


    // @ts-ignore
    $('#datepicker2').datepicker();

    // @ts-ignore
    $(".date-option").on("click", function () {
      // @ts-ignore
      let daysToAdd = $(this).data("days");
      let currentDate = new Date();
      currentDate.setDate(currentDate.getDate() + (daysToAdd ) );
      const formattedDate = currentDate
        .toISOString()
        .split('T')[0]
        .split('-')
        .reverse()
        .join('/');
      // @ts-ignore
      $("#datepicker2").val(formattedDate);
    });

    // @ts-ignore
    $('#followUp-result-list-Container li').on('click', function () {

      // @ts-ignore
      $('#followUp-result-list-Container li').removeClass('active');
      // @ts-ignore
      $(this).addClass('active');

    });

    document.getElementById("modify-order-detail-modal")?.addEventListener("click" , ()=>{
        this.openModifyOrderDetailModal();
    });
  }

  setupPaginationBtnClickListeners(): void {
    const buttons = document.querySelectorAll('button.page-link');
    buttons.forEach((button) => {
      this.renderer.listen(button, 'click', (event:any) => this.onPaginationButtonClick(event));
    });
  }
  onPaginationButtonClick(event : Event){
    const orderId = Number((document.querySelector('.order-card') as HTMLElement).getAttribute('order-id'));
    if((event.target as HTMLElement).classList.contains('next-order')){
      this.GetOrderCardDetail(orderId, "next",this.getSearchFormData());
    }else if((event.target as HTMLElement).classList.contains('prev-order')){
      this.GetOrderCardDetail(orderId, "prev",this.getSearchFormData())
    }
  }

  openModifyOrderDetailModal(){
     this.modalService.open(ModifyOrderDetailModalComponent, {OrderId : this.OrderId , CallBackFunction : this.currentOrder.bind(this)});
  }

  initalizeCancelModelScripts(){
    // @ts-ignore
    $(document).ready(() => {
      let filterMenuList: HTMLElement[] = [];
      // @ts-ignore
      $('#cancel-reasonse-list-Container li').on('click', function () {
        // @ts-ignore
        $('#cancel-reasonse-list-Container li').removeClass('active');
        // @ts-ignore
        $(this).addClass('active');
        // @ts-ignore
        console.log($(this).hasClass('active'));
      });

      function filterMenu(event: Event) {
        let searchValue = (event.target as HTMLInputElement).value.trim().toLowerCase();

        if (filterMenuList.length === 0) {
          // @ts-ignore
          $("#cancel-reasonse-list-Container li").each((_, element) => {
            filterMenuList.push(element);
          });
        }
        // @ts-ignore
        $("#cancel-reasonse-list-Container").empty();

        if (searchValue === "") {
          filterMenuList.forEach(element => {
            // @ts-ignore
            $("#cancel-reasonse-list-Container").append(element);
          });
          rebindClickEvent();
          return;
        }

        filterMenuList.forEach(element => {
          if (element.innerHTML.toLowerCase().includes(searchValue)) {
            // @ts-ignore
            $("#cancel-reasonse-list-Container").append(element);
          }
        });
        rebindClickEvent();
      }

      function rebindClickEvent() {
        // @ts-ignore
        $("#cancel-reasonse-list-Container li").on("click", (event) => {
          // @ts-ignore
          if ($(event.currentTarget).hasClass("active")) return;
          // @ts-ignore
          $("#cancel-reasonse-list-Container li").removeClass("active");
          // @ts-ignore
          $("#cancel-reasonse-list-Container li input").val('');
          event.currentTarget.classList.add("active");
        });
      }
      // @ts-ignore
      $("#cancel-reasonse-search-input").on("input", filterMenu);
    });
  }

  setUpOrderBySectionScripts(){
    let orderASCBtn = (document.getElementById("orderASC") as HTMLButtonElement);
    let orderDECSBtn = (document.getElementById("orderDESC") as HTMLButtonElement);
    orderASCBtn.addEventListener("click", (event) => {
         if(orderDECSBtn.classList.contains('active')) {
           orderDECSBtn.classList.remove('active');
           orderASCBtn.classList.add('active');
         }
    })
    orderDECSBtn.addEventListener("click", (event) => {
      if(orderASCBtn.classList.contains('active')) {
        orderASCBtn.classList.remove('active');
        orderDECSBtn.classList.add('active');
      }
    })
  }
  getSearchFormData() {
    let IsshowSearchSection : boolean = true;
    let HoursSinceLastTransaction : string  = (document.getElementById('HoursSinceLastTransaction') as HTMLFormElement)?.innerText?.trim()??"";
    if((document.getElementById('searchForm') as HTMLFormElement).style.display == "none"){
      IsshowSearchSection = false;
    }
    const form = document.getElementById('searchForm') as HTMLFormElement;
    console.log(form);
    const formData : any = new FormData(form);
    formData.append("IsshowSearchSection" , IsshowSearchSection);
    let sortType = Array.from(
      (document.getElementById("OrderBySection") as HTMLDivElement).children
    ).find(_ => _.classList.contains('active'))?.id.split("order")[1];
    formData.append("SortType" , sortType);
    formData.append("HoursSinceLastTransaction" , HoursSinceLastTransaction)
    const jsonObject = Object.fromEntries(formData?.entries());
    console.log(jsonObject);
    return jsonObject;
  }


}
