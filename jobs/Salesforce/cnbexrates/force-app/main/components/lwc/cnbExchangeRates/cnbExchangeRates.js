import { LightningElement, wire } from 'lwc';
import getExchangeRates from '@salesforce/apex/ExchangeRateController.getExchangeRates';

const columns = [
  { label: 'Code', fieldName: 'currencyCode' },
  { label: 'To CZK', fieldName: 'exactRate', type: 'number' },
  { label: 'From CZK', fieldName: 'invertedRate', type: 'number' }
];

export default class CnbExchangeRates extends LightningElement {
  
  @wire(getExchangeRates) exchangeRates;
  tableHeaders = columns;

  get validityDate() {
    return this.exchangeRates.data?.[0]?.validFor;
  }

}