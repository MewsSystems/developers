import {RateWorkerActionInput, RateWorkerActionOutput} from "../../types/worker";
import {RatesList} from "../../types/app";

const ctx: Worker = self as any;
let rateIntervalId = null;

const getRate = (ids: string[]) => {
    const query = `?${ids.map(id => {
        return `currencyPairIds[]=${id}`
    }).join('&')}`;

    fetch(`${process.env.SERVER_URL}/rates${query}`)
        .then(res => res.json())
        .then((rates: RatesList) => ctx.postMessage({type: RateWorkerActionOutput.success, data: rates.rates}))
        .catch(() => ctx.postMessage({type: RateWorkerActionOutput.failure}));
};

const runRateJob = (ids: string[]) => {
    getRate(ids);
    rateIntervalId = setInterval(() => getRate(ids), process.env.RATE_INTERVAL);
};

const stopRateJob = () => {
    rateIntervalId && clearInterval(rateIntervalId);
    rateIntervalId = null;
};

ctx.addEventListener('message', ({data}) => {
    switch (data.type) {
        case RateWorkerActionInput.start:
            runRateJob(data.ids);
            break;
        case RateWorkerActionInput.stop:
            stopRateJob();
            break;
    }
});