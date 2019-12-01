import { RatesDTO, CurrencyPairConfigDTO } from "../models/DTOs";
import Currency from "../models/Currency";

/**
 *
 * http://localhost:3000/rates?currencyPairIds=70c6744c-cba2-5f4c-8a06-0dac0c4e43a1&currencyPairIds=611398c5-6bd9-596e-8803-3ed0b093995d
 */

const destination = "http://localhost:3000";

/**
 *
 * @param ids Currency ids, which rates needs to be updated
 */
export function getRatesDTO(ids: string[]): Promise<RatesDTO> {
  const createURLquery = (input: string[]): string =>
    input.map(id => "currencyPairIds=" + id).join("&");

  return fetch(destination + "/rates?" + createURLquery(ids), { method: "GET" })
    .then(response => {
      if (response.status === 500) {
        throw new Error("500 status code");
      } else {
        return response.json();
      }
    })
    .then(data => {
      /**Shape of data: 
         * {
            rates: {
                id2: 1.0345 }
          }   
         */
      let rates = data.rates;
      let ids = Object.keys(rates);
      let returnObject: { [key: string]: Number } = {};
      ids.forEach(id => {
        returnObject[id] = rates[id] as Number;
      });

      return returnObject;
    });
}

/**
 * Called to get the config data, returns a promise
 */
export function getConfigDTO(): Promise<CurrencyPairConfigDTO> {
  //DATA SHAPE
  /* {
        currencyPairs: {
            id1: [{ code: 'EUR', name: 'Euro' }, { code: 'USD', name: 'US Dollar' }],
            id2: [{ name: 'GBP', name: 'British Pound' }, { code: 'JPY', name: 'Japanese Yen' }],
            ...
        }
    } */
  return fetch(destination + "/configuration", { method: "GET" })
    .then(response => response.json())
    .then(data => {
      let currencyPair = data.currencyPairs as {
        [id: string]: [{ name: string; code: string }];
      };

      let ids = Object.keys(currencyPair);

      let returnObject: { [id: string]: Currency[] } = {};
      ids.forEach(id => {
        returnObject[id] = currencyPair[id].map<Currency>(currObj => {
          return {
            code: currObj.code,
            name: currObj.name
          };
        });
      });

      return returnObject;
    });
}
