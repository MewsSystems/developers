import { StringTMap, CurrencyPair } from "@store/types";
import { API_URL } from "@constants/config";

let resultStatus = 0;

export interface FetchCurrencyPairsApiResponse {
    currencyPairs:  StringTMap<CurrencyPair>

    // frontend use
    success: boolean;
    errorMessage: string;
}

export const fetchCurrencyPairsApi = (): Promise<FetchCurrencyPairsApiResponse> => (
    fetch(`${API_URL}/configuration`)
    .then(result => {
        resultStatus = result.status;
        return result.json()
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
