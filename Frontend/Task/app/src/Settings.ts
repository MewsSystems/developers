import { CurrencyPairs } from './CurrencyPairs';
import { Rates } from './Rates';

export class Settings {
    private _currencyPairsObjectPointer: CurrencyPairs;

    constructor(currencyPairsObject: CurrencyPairs) {
        this._currencyPairsObjectPointer = currencyPairsObject;
    }
    
    public render() {
        var me = this;
        
        for (var item in me._currencyPairsObjectPointer.currencyPairs) {
            let input = document.createElement("input");
            input.type = "checkbox";
            input.id = "checkbox_" + item;
            
            let label = document.createElement("label");
            (<any>label).for = "checkbox_" + item;
            let pair = me._currencyPairsObjectPointer.currencyPairs[item];
            label.innerHTML = pair[0].code + "/" + pair[1].code;
            label.title = pair[0].name + " / " + pair[1].name;
            label.style.cursor = "help";

            // adding elements
            this._currencyPairsObjectPointer.settingsRenderElement.appendChild(input);
            this._currencyPairsObjectPointer.settingsRenderElement.appendChild(label);
            this._currencyPairsObjectPointer.settingsRenderElement.appendChild(document.createElement("div"));
            // --

            let currencyPairId = item;
            input.onchange = function(e) {
                //always remove
                me._currencyPairsObjectPointer.currencyPairsSelection = me._currencyPairsObjectPointer.currencyPairsSelection.filter(x => x !== currencyPairId);
                if ((<any>this).checked) {
                    // add - remove before for deduplication
                    me._currencyPairsObjectPointer.currencyPairsSelection.push(currencyPairId);
                }
                
                // persist save
                localStorage.setItem("currencyPairsSelection", JSON.stringify(me._currencyPairsObjectPointer.currencyPairsSelection));
                // refresh courses - Rates object will be always defined
                (<Rates>me._currencyPairsObjectPointer.ratesObjectPointer).render();
            }

            // if is in selection
            if (me._currencyPairsObjectPointer.currencyPairsSelection.filter(x => x === currencyPairId).length > 0) {
                input.checked = true;
            }
        }
    }
}