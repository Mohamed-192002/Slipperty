import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import {HubConnection} from "@microsoft/signalr";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  constructor() { }

  public initializeSignalRConnection(connectedUrl : string): HubConnection {
    let signalRHubConnection : HubConnection = new signalR.HubConnectionBuilder()
      .withUrl(connectedUrl)
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.None)
      .build();

    signalRHubConnection
      .start()
      .then(() => {
       // console.log('✅ SignalR Connected!');
      })
      .catch((error) => {
        console.error('❌ SignalR Connection Error:', error);
        setTimeout(() => signalRHubConnection.start().catch(err => console.error('Reconnection attempt failed:', err)), 5000);
      });
    return signalRHubConnection;
  }
  public destroySignalRConnection(connectionHub : HubConnection){
      connectionHub.stop().then(res => {
      })
  }

}
