import {Component, Input, OnInit, Pipe} from '@angular/core';
import {FormBuilder, FormGroup, Validators } from '@angular/forms';
import {OrderService} from "../../../Services/order.service";
import {OrganizationService} from "../../../Services/adress.service";
import {ToasterAlertsService} from "../../../Helper/ToasterAlert";
import {ActionType} from "../../../Entities/ActionType";

interface Government {
  id: number;
  name: string;
}

interface Region {
  id: number;
  name: string;
}






@Component({
  selector: 'app-modify-order-detail-modal',
  templateUrl: './modify-order-detail-modal.component.html',
  styleUrls: ['./modify-order-detail-modal.component.css']
})
export class ModifyOrderDetailModalComponent implements OnInit{
  @Input() OrderId  : number = -1;
  @Input() CallBackFunction : Function = () =>{}
  @Input() close : Function = () => {};

  userAddresses: any = [];
  governments: Government[] = [];
  regions: Region[] = [];
  isAuthenticated = false;
  showError = false;
  selectedGovernment: number | null = null;
  selectedRegion: number | null = null;

  user = {
    firstName: '',
    phoneNumber: '',
    address: '',
    notes: ''
  };

  shippingForm: FormGroup;

  constructor(private orderService: OrderService , private fb: FormBuilder, private lookup : OrganizationService, private alert : ToasterAlertsService) {
    this.shippingForm = this.fb.group({
      fullName: ['', Validators.required],
      phoneNumber: ['', [Validators.required]],
      phoneNumber2 : [''],
      address: ['', Validators.required],
      governmentId: [null, Validators.required],
      regionId: [null, Validators.required],
      notes: [''],
      deliveryTimeFrom : [''],
      deliveryTimeTo : [''],
    });
  }

  ngOnInit(): void {
    this.handleGovernmentChange();
    this.GetGovernments();
    this.GetOrderDetailsData();
    }
  handleGovernmentChange() {
    this.shippingForm.controls['governmentId'].valueChanges.subscribe((value) => {
      this.lookup.GetRegion(value).subscribe(region => {
        this.regions = region;
      })
    })
  }

    private GetGovernments(){
      this.lookup.GetGovernments().subscribe(governments => {
        this.governments = governments;
      })
    }

    private GetOrderDetailsData(){
           this.orderService.GetOrderById(this.OrderId).subscribe(order => {
             //console.log(order);
             this.shippingForm.patchValue(order.orderData);
             this.userAddresses = order.userAddresses;
             console.log(order.orderData);
             console.log(this.shippingForm.value);
           //  console.log(this.userAddresses);
           //  console.log(order.userAddresses);
           },err =>{
             this.alert.error();
           })
    }



  isInvalid(field: string): boolean {
    return this.shippingForm.controls[field].invalid && this.shippingForm.controls[field].touched;
  }

  // Handle form submission
  onSubmit(): void {
    if (this.shippingForm.valid) {

       this.orderService.sendOrderAction(this.OrderId, ActionType.Modified , this.shippingForm.value).subscribe(res => {
         this.alert.success();
         console.log(this.close)
         console.log(this.CallBackFunction);
         if(this.close != null)
            this.close();
         this.CallBackFunction();

       },
       error => {
          this.alert.error();
       });
    } else {
      this.showError = true;
      this.shippingForm.markAllAsTouched();
    }
  }

  getAddressDetails(address: string) {
    // Handle selecting address
    this.shippingForm.get("address")?.setValue(address);

  }

  getRegions() {
    // Handle government change
  }

  checkRegion() {
    // Handle region change
  }



}
