import {Component, ComponentFactoryResolver, ComponentRef, Type, ViewChild, ViewContainerRef} from '@angular/core';

@Component({
  selector: 'app-modal-container',
  template: `
    <div class="modal fade show d-block" tabindex="-1" (click)="onBackdropClick($event)">
      <div class="modal-dialog modal-dialog-centered ">
        <div class="modal-content" style="background-color: #fafafa" >
          <div class="modal-header">
            <button type="button" class="btn-close" (click)="close()" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <ng-container #modalContent></ng-container>
          </div>
        </div>
      </div>
    </div>
    <div class="modal-backdrop fade show"></div>
  `,
  styleUrls: ['./modal-container.component.css'],
})
export class ModalContainerComponent {
  @ViewChild('modalContent', { read: ViewContainerRef, static: true })
  modalContent!: ViewContainerRef;

  constructor(private resolver: ComponentFactoryResolver) {}

  loadComponent<T>(component: Type<T>, data?: any): ComponentRef<T> {
    this.modalContent.clear();
    const factory = this.resolver.resolveComponentFactory(component);
    const componentRef = this.modalContent.createComponent(factory);
    if (data && componentRef.instance) {
      Object.keys(data).forEach((key) => {
        if ((componentRef.instance as any)[key] !== undefined) {
            console.log(key);
            (componentRef.instance as any)[key] = (data as any)[key];
        }
      });
      if((componentRef.instance as any)['close'] !== undefined ) {
          (componentRef.instance as any)['close'] = this.close;
      }
    }

    return componentRef;
  }

  close() {
    document.body.removeChild(document.querySelector('app-modal-container')!);
  }
  onBackdropClick(event: MouseEvent) {
    if (event.target === event.currentTarget) {
      this.close();
    }
  }


}
