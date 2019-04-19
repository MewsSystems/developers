import { endpoint, interval } from '../config.json';
import Singleton from "./abstracts/Singleton"

class AppCore extends Singleton {
    private _endpointUrl: string;

    constructor() {
        super();
        this._endpointUrl = endpoint.replace("/rates", "");
    }

    public get endpointUrl(): string {
        return this._endpointUrl;
    }

}

export = AppCore