import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'app-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.css']
})
export class ConfirmationComponent {
  @Input() Message : string = "هل انت متأكد";
  // @Output() ConfirmationRes : EventEmitter<boolean> = new EventEmitter(false);

  @Input() callBackFunction : Function = () => {}
  @Input() close : Function = () => {}


  confirmed(){
    this.callBackFunction();
    this.close();
  }

  cancel(){
    this.close();
  }


}
