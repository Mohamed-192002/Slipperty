import {Component, EventEmitter, forwardRef, Input, OnChanges, Output, SimpleChanges} from '@angular/core';
import {ControlValueAccessor, NG_VALUE_ACCESSOR} from "@angular/forms";

@Component({
  selector: 'app-time-input',
  template: `
    <div class="d-flex gap-1 justify-content-evenly">
      <input type="number" class="form-control" [(ngModel)]="hour" min="1" max="12" (change)="onTimeChange()" placeholder="ساعة">
      <input type="number" class="form-control" [(ngModel)]="minute" min="0" max="59" (change)="onTimeChange()" placeholder="دقيقة">
      <select class="form-control" [(ngModel)]="period" (change)="onTimeChange()">
        <option value="ص">ص</option>
        <option value="م">م</option>
      </select>
    </div>
  `,
  styleUrls: ['./time-input.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TimeInputComponent),
      multi: true
    }
  ]
})
export class TimeInputComponent implements ControlValueAccessor {

  hour: number = 10;
  minute: number = 0;
  period: string = 'ص';

  private onChange: any = () => {};
  private onTouched: any = () => {};

  writeValue(value: string): void {
    if (value) {
      this.parseTime(value);
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {

  }

  private parseTime(timeStr: string) {
    const timeParts = timeStr.match(/(\d{1,2}):(\d{2})\s?(ص|م)/);
    if (timeParts) {
      this.hour = +timeParts[1];
      this.minute = +timeParts[2];
      this.period = timeParts[3];
    }
  }

  onTimeChange() {
    const formattedTime = `${this.hour}:${this.minute.toString().padStart(2, '0')} ${this.period}`;
    this.onChange(formattedTime);
    this.onTouched();
  }

}
