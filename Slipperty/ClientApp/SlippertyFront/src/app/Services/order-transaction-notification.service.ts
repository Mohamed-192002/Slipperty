import { Injectable } from '@angular/core';
import {SignalRService} from "./signal-r.service";
import {HubConnection} from "@microsoft/signalr";

@Injectable({
  providedIn: 'root'
})
export class OrderTransactionNotificationService {
  private hubConnection !: HubConnection
  constructor(private  signalRConnection: SignalRService) {
      this.hubConnection = this.signalRConnection.initializeSignalRConnection("/notifyIOrderTransaction");
  }

  public ReceiveNewOrderNotification(callBack : Function = () => {}){
    this.hubConnection.on("ReceiveNewOrderNotification" , (data : any) => {
       callBack(data);
    });
  }

  public ReceiveOrderTransaction(callBack : Function){
       this.hubConnection.on("ReceiveOrderTransaction" , (response : any) =>{
           callBack(response);
       });
  }
  public DestroyHubConnection() : void {
    this.signalRConnection.destroySignalRConnection(this.hubConnection);
  }

}
