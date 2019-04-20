import EventAggregator from "../app/src/EventAggregator";
import EEventMessages from '../app/src/abstracts/EEventMessages';

it('EventAggregator subscribe and notifications', () => {
    const ea = EventAggregator.instance;
    
    var notifyResult = function(message, data) {
        expect(message).toBe(EEventMessages.MenuTabSelected);
        expect(data).toEqual({ tab: "settings" });
    }
    ea.subscribe(notifyResult);
    ea.notify(EEventMessages.MenuTabSelected, { tab: "settings"});
});
