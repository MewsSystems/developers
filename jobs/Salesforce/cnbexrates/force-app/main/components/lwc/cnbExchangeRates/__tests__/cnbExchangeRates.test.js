import { createElement } from "lwc";
import CnbExchangeRates from "c/cNBExchangeRates";
import getExchangeRates from "@salesforce/apex/ExchangeRateController.getExchangeRates";

jest.mock(
  "@salesforce/apex/ExchangeRateController.getExchangeRates",
  () => {
    const { createApexTestWireAdapter } = require("@salesforce/sfdx-lwc-jest");
    return {
      default: createApexTestWireAdapter(jest.fn())
    };
  },
  { virtual: true }
);

const exchangeRatesMock = require("./data/exchangeRates.json");
const exchangeRatesErrorMock = require("./data/exchangeRatesError.json");
const flushPromises = () => new Promise(process.nextTick);

describe("c-cnb-exchange-rates", () => {
  afterEach(() => {
    // The jsdom instance is shared across test cases in a single file so reset the DOM
    while (document.body.firstChild) {
      document.body.removeChild(document.body.firstChild);
    }
  });

  it("checks the list of exchange rates is displayed correctly", async () => {
    const element = createElement("c-cnb-exchange-rates", {
      is: CnbExchangeRates
    });
    getExchangeRates.emit(exchangeRatesMock);
    document.body.appendChild(element);

    await flushPromises();
    const validity = element.shadowRoot.querySelector(
      ".slds-scoped-notification"
    );
    expect(validity.textContent).toBe(
      "Exchange rates valid for 2023-05-16. " +
        "The data for the current working day are available after 14:30."
    );

    const dataTable = element.shadowRoot.querySelector("lightning-datatable");
    expect(dataTable.data.length).toBe(31);
    expect(dataTable.data[0].currencyCode).toBe("AUD");
    expect(dataTable.data[0].exactRate).toBe(14.52);

    const error = element.shadowRoot.querySelector(".slds-notify");
    expect(error).toBeFalsy();
  });

  it("checks that a server error is handled correctly", async () => {
    const element = createElement("c-cnb-exchange-rates", {
      is: CnbExchangeRates
    });
    getExchangeRates.error(exchangeRatesErrorMock, 500, "Server Error");
    document.body.appendChild(element);

    await flushPromises();
    const dataTable = element.shadowRoot.querySelector("lightning-datatable");
    expect(dataTable).toBeFalsy();

    const error = element.shadowRoot.querySelector(".slds-notify");
    expect(error.textContent).toBe(
      "VALIDATION_ERROR: ValidationErrorCode: typeMismatch;  Field: lang; Value: i_am_not_valid_lang"
    );
  });

  it("checks that a message is displayed when the exchange rates list is empty", async () => {
    const element = createElement("c-cnb-exchange-rates", {
      is: CnbExchangeRates
    });
    getExchangeRates.emit([]);
    document.body.appendChild(element);

    await flushPromises();
    const dataTable = element.shadowRoot.querySelector("lightning-datatable");
    expect(dataTable.data.length).toBe(0);

    const noData = element.shadowRoot.querySelector(".no-data");
    expect(noData.textContent).toBe("No data to display.");

    const error = element.shadowRoot.querySelector(".slds-notify");
    expect(error).toBeFalsy();
  });
});
