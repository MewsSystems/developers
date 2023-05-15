import { LightningElement, wire } from 'lwc';
import getExchangeRates from '@salesforce/apex/ExchangeRateController.getExchangeRates';

const columns = [
    { label: 'Country', fieldName: 'country' },
    { label: 'Currency Name', fieldName: 'currencyName' },
    { label: 'Quantity', fieldName: 'quantity', type: 'number' },
    { label: 'Code', fieldName: 'code' },
    { label: 'Exchange Rate', fieldName: 'exchangeRate', type: 'number', cellAttributes: { alignment: 'left' } }
];

export default class ExchangeRateLWC extends LightningElement {
    exchangeRates;
    columns = columns;
    exchangeRateDate;

    @wire(getExchangeRates)
    fetchExchangeRates({ error, data }) {
        if (data) {
            this.exchangeRates = data;
            this.exchangeRateDate = data[0].exchangeRateDate;
        } else if (error) {
            console.error('Error retrieving exchange rates:', error);
        }
    }
}