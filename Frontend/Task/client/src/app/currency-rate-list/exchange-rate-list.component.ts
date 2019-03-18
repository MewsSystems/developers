import { Component, OnInit } from '@angular/core';
import { Status } from '../models/status.model';
import { StorageKeys } from '../models/storage-keys.model';
import { ExchangeRateService } from '../services/exchange-rate.service';
import { ExchangeRate } from '../models/exchange-rate.model';

@Component({
    selector: 'exchange-rate-list',
    templateUrl: './exchange-rate-list.component.html',
    styleUrls: ['./exchange-rate-list.component.css']
})
export class ExchangeRateListComponent implements OnInit {

    exchangeRateList: ExchangeRate[] = [];
    selectedIds: string[] = [];
    updateInterval = 10000;
    updateIntervalFuntion: any;
    errorMessage: string;

    constructor(private exchangeRateService: ExchangeRateService){}

    ngOnInit() {
        var exchangeRateList = localStorage.getItem(StorageKeys.exchangeRateList);

        if(exchangeRateList){
            this.initSavedData(exchangeRateList);
        } else {
            this.initNewData();
        }
    }

    initSavedData(configuration: string ) {
        this.exchangeRateList = JSON.parse(configuration);

        var selectedIds = localStorage.getItem(StorageKeys.selectedIds);

        if(selectedIds) {
            this.selectedIds = JSON.parse(selectedIds);
        } else {
            this.selectedIds = this.exchangeRateList.map(({ id }) => id);
            localStorage.setItem(StorageKeys.selectedIds, JSON.stringify(this.selectedIds));
        }
        this.updateRates();
        this.startUpdateByInterval();
    }

    initNewData() {
        this.exchangeRateService.getExchangeRateList().subscribe(
            (data: ExchangeRate[]) => {
                this.errorMessage = null;
                this.exchangeRateList = data;
                localStorage.setItem(StorageKeys.exchangeRateList, JSON.stringify(this.exchangeRateList));
                this.selectedIds = data.map(({ id }) => id);
                localStorage.setItem(StorageKeys.selectedIds, JSON.stringify(this.selectedIds));

                this.updateRates();
                this.startUpdateByInterval();
            },
            (error: any) => {
                this.errorMessage = error.statusText;
                console.log(error);
            }
        );
    }


    startUpdateByInterval() {
        this.updateIntervalFuntion = setInterval(() => {
            this.updateRates();
        }, this.updateInterval);
    }

    updateRates() {
        if(this.selectedIds && this.selectedIds.length) {
            this.exchangeRateService.getRates(this.selectedIds).subscribe(
                (result: any) => {
                    this.errorMessage = null;
                    this.fillRates(result);
                },
                (error: any) => {
                    this.errorMessage = error.statusText;
                    console.log(error);
                }
            );
        }
    }

    fillRates(rates: any) {
        for(let rateId of Object.keys(rates)) {
            let newRate = rates[rateId];
            let exchangeRate = this.exchangeRateList.find(i => i.id === rateId);
            this.setStatus(exchangeRate, newRate);
            exchangeRate.rate = newRate;
        }
    }

    setStatus(exchangeRate: ExchangeRate, newRate: number){
        if(exchangeRate.rate < newRate) {
            exchangeRate.status = Status.growing;
        } else if(exchangeRate.rate > newRate) {
            exchangeRate.status = Status.declining;
        } else {
            exchangeRate.status = Status.stagnating;
        }
    }

    getFilterdRates(): ExchangeRate[] {
        const me = this;

        return this.exchangeRateList.filter(function (rate) {
            const selectedRate = me.selectedIds.find(i => i === rate.id)
            if(selectedRate) {
                return true;
            }
        });
    }

    getRowClass(exchangeRate: ExchangeRate) {
        if(exchangeRate.status === Status.growing) {
            return 'growingRow'
        } else if (exchangeRate.status === Status.declining) {
            return 'decliningRow'
        } else {
            return 'stagnatingRow'
        }
    }

    onUpdateRatesClick() {
        if (this.updateIntervalFuntion) {
            clearInterval(this.updateIntervalFuntion);
        }

        this.updateRates();
        this.startUpdateByInterval();
    }

    onFilterChanged() {
        localStorage.setItem(StorageKeys.selectedIds, JSON.stringify(this.selectedIds));
    }
}