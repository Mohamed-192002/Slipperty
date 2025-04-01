import { Injectable } from '@angular/core';
import {Observable, Observer} from "rxjs";
import {HttpClient, HttpParams} from "@angular/common/http";
import {AppConfigService} from "./app-config.service";

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private  baseUrl : string | null = AppConfigService.config.apiUrl;
   constructor(private httpClient : HttpClient) { }

   public Post (url : string , data : any = null, options : any = { responseType: 'json' }): Observable<any>{
     return this.httpClient.post<any>(this.baseUrl + url, data, options)
   }

  public Get (url : string ,  options : any = { responseType: 'json' }, data : any = null): Observable<any>{
    const httpParams = new HttpParams({ fromObject: data });
    return this.httpClient.get<any>(this.baseUrl + url, {...options,httpParams} )
  }

  public Put (url : string , data : any = null, options : any = { responseType: 'json' }): Observable<any>{
    return this.httpClient.put<any>(this.baseUrl + url, data, options)
  }

  public delete (url : string ,  options : any = { responseType: 'json' }): Observable<any>{
    return this.httpClient.delete<any>(this.baseUrl + url,  options)
  }
}
