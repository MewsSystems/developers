import { CurrencyPair } from "../store/types";

const API_URL = 'http://localhost:3000';
let resultStatus = 0;

export interface FetchRatesApiResponse {
    rates: CurrencyPair[]

    // frontend use
    success: boolean;
    errorMessage: string;
}

export const fetchRatesApi = (currencyPairIds: string[]): Promise<FetchRatesApiResponse> => (
    fetch(`${API_URL}/rates?currencyPairIds=[${currencyPairIds[0]}]`)
    .then(result => {
        resultStatus = result.status;
        if(resultStatus == 500) {
            return ""
        } else {
            return result.json();
        }

    })
    .then(resultJSON => {
        if (resultStatus >= 200 && resultStatus < 300) {
            return {
                ...resultJSON,
                success: true
            };
        } else if (resultStatus >= 400 && resultStatus < 500) {
            return {
                ...resultJSON,
                success: false,
                errorMessage: resultJSON.detail || "Something went wrong, please try again later"
            }
        } else if (resultStatus == 500) {
            return {
                ...resultJSON,
                success: false,
                errorMessage: "Error in connection."
            }
        }
    })
    .catch(err => {
        return {
            success: false,
            errorMessage: "Something went wrong.",
        }
    })
);
