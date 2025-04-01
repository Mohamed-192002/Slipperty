import {
  AfterViewInit,
  Component,
  ComponentFactoryResolver,
  HostListener, Injector,
  OnInit,
  Renderer2,
  ViewChild,
  ViewContainerRef
} from '@angular/core';
import {OrderService} from "../Services/order.service";
import {ErrorDuringLoadingData, LoadingSpinner} from "../Helper/html-helpers";
import {ToasterAlertsService} from "../Helper/ToasterAlert";
import { OrderCardDetailComponent } from './order-card-detail/order-card-detail.component';
import {ModalService} from "../Services/modal.service";
import {
  AddOrderDetailProductComponent
} from "../orders/OrderModals/add-order-detail-product/add-order-detail-product.component";

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements  OnInit, AfterViewInit{
  @ViewChild('ajaxModal', { read: ViewContainerRef }) modalContainer!: ViewContainerRef;
  @ViewChild('randerComponent', {read : ViewContainerRef, static: true}) randerComponent!: ViewContainerRef;
  constructor(private  injector : Injector,private orderService : OrderService, private renderer: Renderer2, private alert : ToasterAlertsService, private resolver: ComponentFactoryResolver , private modal : ModalService) {
  }

    ngOnInit(): void {
      this.setupTabClickListeners();
      this.loadActiveTabContent();

    }


  ngAfterViewInit() : void{
    this.orderService.setModalContainer(this.modalContainer);
  }

  // @HostListener('click', ['$event'])
  // public paginate(event: Event) {
  //   const target = event.target as HTMLElement;
  //   console.log(target);
  //   if (target.classList.contains('page-link')) {
  //     if(target.classList.contains('next-order')){
  //       this.nextOrder();
  //     }else if(target.classList.contains('prev-order')){
  //       console.log('previous order Not Implemented yet');
  //     }
  //
  //   }
  // }
  @HostListener('click' , ['$event'])
  public paginate(event : Event){
    const target = event.target as HTMLElement;
    debugger;
    if((!target.classList.contains("next-order") && !target.classList.contains("prev-order") && !target.parentElement?.classList.contains("active")) && (target.classList.contains("page-link") || target.classList.contains("order-btn"))){
       // @ts-ignore
        this.loadTabContent(window.getTabStatus((document.querySelector('.tab-active-color') as HTMLElement).id), Number(target.getAttribute("data-page")),this.getSearchFormData());
    }
  }
  private getSearchFormData():string{

    const form = document.getElementById('searchForm') as HTMLFormElement;
    const formData : any = new FormData(form);
    const jsonObject : any = Object.fromEntries(formData?.entries());
    jsonObject.OrderNos = Array.from(
      (document.querySelector('#valuesField') as HTMLInputElement).value
        .split(',')
        .filter(orderNo => orderNo.trim() !== "")
    );    console.log(jsonObject);
    console.log(JSON.stringify(jsonObject));
    return "queryString="+JSON.stringify(jsonObject);
  }

  setupTabClickListeners(): void {
    const tabs = document.querySelectorAll('.tab-color');
    tabs.forEach((tab) => {
      this.renderer.listen(tab, 'click', (event:any) => this.onTabClick(event));
    });
  }




  setupTableClickListeners(): void {
    const rows = document.querySelectorAll('.table tr .order-no'); // Select all rows in the table
    rows.forEach((row) => {
      this.renderer.listen(row, 'click', (event) => this.onTableRowClick(event));
    });
  }
  onTableRowClick(event: any): void {
    const orderId = event.target.closest('tr')?.getAttribute('order-id') ?? -1;
    const tabId = document.querySelector('.tab-active-color')?.id??'';
    const status = this.getTabStatus(tabId);
    if (orderId && status && orderId != -1) {
      this.randerComponent.clear();
        let factory = this.resolver.resolveComponentFactory(OrderCardDetailComponent);
       let component =   this.randerComponent.createComponent(factory);
        component.instance.OrderId = orderId;
        component.instance.status = status;
        component.instance.LoadTabList = this.loadActiveTabContent.bind(this)
        // this.GetOrderCardDetail(orderId, status);
    }
  }




  onTabClick(event: any): void {
    const tabId = event.target.id;


    const previousActiveTab = document.querySelector('.tab-active-color');
    if (previousActiveTab) {
      previousActiveTab.classList.remove('tab-active-color');
    }


    event.target.classList.add('tab-active-color');


    // @ts-ignore
    const status = window.getTabStatus(tabId);

    document.getElementById('card-pagintation')?.setAttribute('style', 'display: none');
    if (status) {
      this.loadTabContent(status);
    }
  }
  getTabStatus(tabId: string): string | null {

    return tabId ? tabId : null;
  }


  loadTabContent(status: string, page?: number, queryString?: string): void {
    const container = document.getElementById('partial-container');
    if (container) {
      container.innerHTML = LoadingSpinner;
    }
    this.orderService.GetOrders(Number(status),page, queryString).subscribe({

      next : (data) => {
        if (container) {
          container.innerHTML = data;
          this.setupTableClickListeners();
          this.setUpShowFilterScript();
          this.setUpMultiInputsScripts();
        }
      },
      error:(error) => {
        console.log(error);
        if (container) {
          container.innerHTML = ErrorDuringLoadingData;
        }
      }
     }
    );
  }


  loadActiveTabContent(): void {
    const activeTab = document.querySelector('.tab-color.active');
    if (activeTab) {
      const activeTabId = activeTab.id;
      activeTab.classList.add('tab-active-color');

      // @ts-ignore
      const initialStatus = window.getTabStatus(activeTabId);
      if (initialStatus) {
        this.loadTabContent(initialStatus);
      }
    }
  }

  setUpShowFilterScript(){
    // @ts-ignore
    $('#OrderDate').datepicker();

    // @ts-ignore
    $("#toggleFormBtn").click(function () {
      // @ts-ignore
      $("#searchForm").slideToggle();
      // @ts-ignore
      $(this).text(($("#searchForm").is(":visible")) ? "Hide Filters" : "Show Filters");
    });

  }

  setUpMultiInputsScripts(){
    const inputField = document.querySelector('#inputField') as HTMLInputElement;
    const valuesField = document.querySelector('#valuesField') as HTMLInputElement;
    const chipsContainer = document.querySelector('#chipsContainer') as HTMLElement;

    if (!inputField || !valuesField || !chipsContainer) return;

    let values = valuesField.value ? valuesField.value.split(',') : [];

    inputField.addEventListener('keydown', (event: KeyboardEvent) => {
      event.stopPropagation();
      if (event.key === 'Enter' && inputField.value.trim()) {
        event.preventDefault();
        let newValue = inputField.value.trim();
        if (!values.includes(newValue)) {
          values.push(newValue);
          updateView();
        }
        inputField.value = '';
      }
    });
    document.querySelectorAll('.remove-chip').forEach(button => {
      button.addEventListener('click', (event: Event) => {
        event.stopPropagation();
        const buttonElement = event.target as HTMLButtonElement;
        const index = parseInt(buttonElement.getAttribute('data-index')!, 10);
        values.splice(index, 1);
        updateView();
      });
    });


    function updateView() {
      chipsContainer.innerHTML = '';
      values.forEach((value, index) => {
        let chip = document.createElement('span');
        chip.classList.add('chip');
        chip.innerHTML = `${value} <button type="button" class="remove-chip">×</button>`;
        chipsContainer.appendChild(chip);

        // Attach remove event
        chip.querySelector('.remove-chip')?.addEventListener('click', (event: Event) => {
          event.stopPropagation();
          values.splice(index, 1);
          updateView();
        });
      });

      valuesField.value = values.join(',');
    }
  }





}
