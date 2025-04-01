import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {AppConfig} from "../Entities/AppConfig";
import {catchError, Observable, of, tap} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {
  public static config: AppConfig = {apiUrl : ""};
  constructor(private httpClient : HttpClient) { }

  loadConfig(): Observable<any> {
    return this.httpClient.get('/js/dist/assets/config/app-config.json').pipe(
      tap((data) => {
        console.log(data);
        AppConfigService.config = data as AppConfig;
      }),
      catchError((error) => {
        console.error('Could not load app config', error);
        return of(null);
      })
    );
  }


}
