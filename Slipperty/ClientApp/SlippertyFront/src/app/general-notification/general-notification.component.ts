import {Component,  OnInit} from '@angular/core';
import {OrderTransactionNotificationService} from "../Services/order-transaction-notification.service";
import {ToasterAlertsService} from "../Helper/ToasterAlert";

@Component({
  selector: 'app-general-notification',
  templateUrl: './general-notification.component.html',
  styleUrls: ['./general-notification.component.css']
})
export class GeneralNotificationComponent implements  OnInit{
  private isAudioUnlocked !: boolean;
  private audioElement !: HTMLAudioElement;
  constructor(private orderTransactionNotificaion : OrderTransactionNotificationService, private alert : ToasterAlertsService) {}

  ngOnInit(): void {
       this.audioElement = document.getElementById('audioPlayer') as HTMLAudioElement;
       document.body.addEventListener("click", () => {
        if (!this.isAudioUnlocked) {
          this.audioElement.play().then(() => {
            this.audioElement.pause();
            this.audioElement.currentTime = 0;
            this.isAudioUnlocked = true;
          }).catch(error => console.error("Audio unlock failed:", error));
        }
       }, { once: true });
       this.orderTransactionNotificaion.ReceiveNewOrderNotification(this.NotifiyNewOrder.bind(this));
    }
  playNotificationVoice() {
    if (this.isAudioUnlocked) {
      this.audioElement.play().catch(error => {
        console.error("Playback blocked:", error);
      });
    }
  }

    public NotifiyNewOrder(response : any){
        this.playNotificationVoice();
        this.alert.NewOrderNotification();
    }

}
