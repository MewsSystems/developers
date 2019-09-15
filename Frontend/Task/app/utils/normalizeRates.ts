import { getTrend } from "./getTrend";

export const normalizeRates = (ratesOldState: any, ratesFetchData: any) => {
    let ratesIds: string[] = Object.keys(ratesFetchData);
    const normalizedRates: any = {};

    ratesIds.map(id => {
        normalizedRates[id] = {
            value: ratesFetchData[id],
            trend: getTrend(ratesOldState && ratesOldState[id] && ratesOldState[id].value || 0, ratesFetchData[id]),
        }
    });

    return normalizedRates;
}