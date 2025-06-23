import { LightningElement, wire } from 'lwc';
import {ShowToastEvent} from 'lightning/platformShowToastEvent';
import getActiveCurrencies from '@salesforce/apex/ExchangeRatesPanelController.getActiveCurrencies';

const columns = [
    { label: 'Currency', fieldName: 'IsoCode', type: 'text' },
    { label: 'ConversionRate', fieldName: 'ConversionRate', type: 'currency' ,cellAttributes: { alignment: 'left' } },
    { label: 'To date', fieldName: 'LastModifiedDate', type: 'date' , cellAttributes: { alignment: 'left' } }
];

export default class ExchangeRatesPanel_LWC extends LightningElement {
    columns = columns;
    dataList =[];
    @wire(getActiveCurrencies)
    wiredData({error , data} ){
        if (data){
            this.dataList = data;
            console.log('Data:', data);
        }
        else if (error){
           console.error('Error:', error);
        }
    }
}