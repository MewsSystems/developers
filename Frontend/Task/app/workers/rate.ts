import Worker from 'worker-loader!./rate.worker';
import {RateWorkerActionInput, RateWorkerActionOutput} from "../../types/worker";

const worker = new Worker();

let sendData = null;
let sendError = null;

const dataListener = ({data}) => {
    switch (data.type) {
        case RateWorkerActionOutput.success:
            sendData && sendData(data.data);
            break;
        case RateWorkerActionOutput.failure:
            sendError && sendError();
            break;
    }
};

export const start = (ids: string[], dataHandler: any, errorHandler: any) => {
    sendData = dataHandler;
    sendError = errorHandler;

    worker.postMessage({type: RateWorkerActionInput.start, ids});
    worker.addEventListener('message', dataListener);
};

export const stop = () => {
    worker.postMessage({type: RateWorkerActionInput.stop});
    worker.removeEventListener('message', dataListener);
};
