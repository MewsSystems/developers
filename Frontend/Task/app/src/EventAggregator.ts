import Singleton from './abstracts/Singleton';
import EEventMessages from './abstracts/EEventMessages'

class EventAggregator extends Singleton {
    private _subscribed: Array<Function> = [];

    public subscribe(remoteFunction: Function) {
        this._subscribed.push(remoteFunction);
    }

    public unsubscribe(remoteFunction: Function) {
        this._subscribed = this._subscribed.filter(function(x) { return x !== remoteFunction });
    }

    public notify(eventName: EEventMessages, value: any = {}) {
        for (var item of this._subscribed) {
            item(eventName, value);
        }
    }
}

export = EventAggregator