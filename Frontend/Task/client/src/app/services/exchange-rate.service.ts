import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { CustomHttpUrlEncodingCodec } from '../share/CustomHttpUrlEncodingCodec';
import { ExchangeRate } from '../models/exchange-rate.model';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class ExchangeRateService {

    serverUrl = 'http://localhost:3000/';

    constructor(private http: HttpClient) { }

    getExchangeRateList(): Observable<ExchangeRate[]> {
        return this.http.get(this.serverUrl + 'configuration').pipe(
            map((response: any)=> {
                return this.converToExchangeRateList(response.currencyPairs)
            })
        );;
    }

    getRates(ids: string[]): any {
        if(!ids || !ids.length)
        {
            return;
        }

        return this.http.get(this.serverUrl + 'rates', 
            {
                params: this.getHttpParams(ids)
            }).pipe(
                map((response: any)=> {
                    return response.rates;
                })
            );
    }

    private getHttpParams(ids: string[]): HttpParams {
        let params: HttpParams = new HttpParams({
            encoder: new CustomHttpUrlEncodingCodec()
        });

        ids.forEach(element => {
            params = params.append('currencyPairIds[]', element);
        });

        return params;
    }

    private converToExchangeRateList(response: any): ExchangeRate[] {
        let result = [];

        for(let pairId of Object.keys(response)) {

            let currencyPair = new ExchangeRate();
            currencyPair.id = pairId;
            currencyPair.sourceCurrency = response[pairId][0];
            currencyPair.targetCurrency = response[pairId][1];

            result.push(currencyPair);
        }

        return result;
    }
}