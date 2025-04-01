import {ApplicationRef, ComponentFactoryResolver, ComponentRef, Injectable, Injector, Type} from '@angular/core';
import { ModalContainerComponent } from '../shared/modal-container/modal-container.component';

@Injectable({
  providedIn: 'root'
})
export class ModalService {

  private modalRef!: ComponentRef<ModalContainerComponent>;

  constructor(
    private resolver: ComponentFactoryResolver,
    private appRef: ApplicationRef,
    private injector: Injector
  ) {}

  open<T>(component: Type<T>, data?: Partial<T>): ComponentRef<T> {

    const factory = this.resolver.resolveComponentFactory(ModalContainerComponent);
    this.modalRef = factory.create(this.injector);


    this.appRef.attachView(this.modalRef.hostView);
    document.body.appendChild(this.modalRef.location.nativeElement);


    return this.modalRef.instance.loadComponent(component, data);
  }

  close() {
    if (this.modalRef) {
      this.appRef.detachView(this.modalRef.hostView);
      this.modalRef.destroy();
    }
  }



}
