import { Rate } from "@store/types";
import { API_URL } from "@constants/config";

let resultStatus = 0;

export interface FetchRatesApiResponse {
    rates: Rate[]

    // frontend use
    success: boolean;
    errorMessage: string;
}

export const fetchRatesApi = (currencyPairIds: string[]) => (
    fetch(`${API_URL}/rates?currencyPairIds[]=${currencyPairIds.join('&currencyPairIds[]=')}`)
    .then(result => {
        resultStatus = result.status;
        if(resultStatus >= 200 && resultStatus < 300) {
            return result.json();
        } else {
            return ""
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
