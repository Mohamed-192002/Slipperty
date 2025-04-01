import { Injectable } from '@angular/core';
import {ApiService} from "./api.service";

@Injectable({
  providedIn: 'root'
})
export class OrganizationService {
   readonly ApiUrl : string = "api/LookUps";
  constructor(private api : ApiService) { }

  GetRegion(GovernmentId:number | null){
    return this.api.Get(`${this.ApiUrl}/GetRegion/${GovernmentId}`);
  }

  GetGovernments(){
    return this.api.Get(`${this.ApiUrl}/GetGovernments`);
  }



}
