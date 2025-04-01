import { Injectable } from '@angular/core';
import {ApiService} from "./api.service";
import {Page} from "../Entities/Page";

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private api : ApiService) { }

  public GetProducts(data : Page){
    return this.api.Post('GetAllProduct',data);
  }

  public GetProductById(id:number){
    return this.api.Get(`GetProductById/${id}`);
  }

}
