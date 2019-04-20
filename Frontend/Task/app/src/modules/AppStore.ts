import Store from '../abstracts/store'
import EventAggregator from '../EventAggregator';
import EEventMessages from '../abstracts/EEventMessages';

class AppStore extends Store {
    private _events: EventAggregator = EventAggregator.instance as EventAggregator;

    constructor() {
        super();
        this._events.subscribe(this.notifyAction.bind(this));
    }

    private notifyAction(message: EEventMessages, value: any) {
        if (message == EEventMessages.MenuTabSelected) {
            this.setTab(value.tab);
        }
    }

    public currencyGet(): Array<string> {
        let response: Array<string> = [];
        try {
            response = this.get("currencyPairsSelection");
        } catch (e) {
            console.error("store-expection");
        }

        return response;
    }

    public currencyAdd(key: string) {
        let data = this.currencyGet();
        data = data.filter(function(x) { return x !== key });
        data.push(key);
        window.localStorage.setItem("currencyPairsSelection", JSON.stringify(data));
    }

    public currencyRemove(key: string) {
        let data = this.currencyGet();
        data = data.filter(function(x) { return x !== key });
        window.localStorage.setItem("currencyPairsSelection", JSON.stringify(data));
    }

    public getTab(): string {
        let data = this.get("currencyPairsActiveTab");
        // default value
        if (data !== "settings" && data !== "rates")  {
            data = "settings";
        }
        return data;
    }

    public setTab(tab: "settings" | "rates") {
        this.set("currencyPairsActiveTab", tab);
    }
}

export = AppStore