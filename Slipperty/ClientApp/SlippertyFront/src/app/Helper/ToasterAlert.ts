import {Injectable} from "@angular/core";
@Injectable({
  providedIn: 'root'
})
export class ToasterAlertsService {
   public success
     (Msg : string = "تمت العملية بنجاح" , option =  {positionClass: 'toast-top-right'})
   {
       // @ts-ignore
       toastr.options = option
       // @ts-ignore
       toastr.success(Msg);
   }
   public error = (Msg : string = "عفوا حدث خطأ", option =  {positionClass: 'toast-top-right'}) => {
     // @ts-ignore
     toastr.options = option
     // @ts-ignore
     toastr.error(Msg);
   }


  public NewOrderNotification = (Msg : string = "تم اضافة طلب جديد", option =  {positionClass: 'toast-bottom-right', toastClass: "custom-toast"}) => {
    // @ts-ignore
    toastr.options = option
    // @ts-ignore
    toastr.success(Msg);
  }

}
