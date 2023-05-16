import { LightningElement, wire } from "lwc";
import getExchangeRates from "@salesforce/apex/ExchangeRateController.getExchangeRates";

const columns = [
  { label: "Code", fieldName: "currencyCode" },
  { label: "To CZK", fieldName: "exactRate", type: "number" },
  { label: "From CZK", fieldName: "invertedRate", type: "number" }
];

/**
 * Component to list the current exchange rates from CNB.
 */
export default class CnbExchangeRates extends LightningElement {

  /** @property {Object} exchangeRates Calculated CNB exchange rate data */
  @wire(getExchangeRates) exchangeRates;

  /** @property {Array} tableHeaders Exchange rate table headers */
  tableHeaders = columns;

  /** @property {string} validityDate Date when the current data is valid */
  get validityDate() {
    return this.exchangeRates.data?.[0]?.validFor;
  }
}
