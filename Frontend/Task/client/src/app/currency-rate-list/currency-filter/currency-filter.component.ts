import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ExchangeRate } from 'src/app/models/exchange-rate.model';

@Component({
    selector: 'currency-filter',
    templateUrl: './currency-filter.component.html',
    styleUrls: ['./currency-filter.component.css']
})
export class CurrencyFilterComponent {

    @Input() exchangeRateList: ExchangeRate[];
    @Input() selectedIds: ExchangeRate[];
    @Output() selectedIdsChange = new EventEmitter();

    @Output() filterChanged: EventEmitter<any> = new EventEmitter();
    @Output() updateRatesClick: EventEmitter<any> = new EventEmitter();

    onSelectedIdsChange() {
        this.selectedIdsChange.emit(this.selectedIds);
        this.filterChanged.emit(this.selectedIds);
    }
    
    onUpdateRatesClick() {
        this.updateRatesClick.emit(this.selectedIds);
    }
}