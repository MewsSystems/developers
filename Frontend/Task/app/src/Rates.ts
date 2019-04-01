import { endpoint, interval } from '../config.json';
import { CurrencyPairs } from './CurrencyPairs';

export class Rates {
    private _currencyPairsObjectPointer: CurrencyPairs;
    private _resultRenderPanel: HTMLDivElement;
    private _errorRenderPanel: HTMLDivElement;

    private _cachedResult: { [s: string]: number; } = {};

    constructor(currencyPairsObject: CurrencyPairs) {
        var me = this; 
        this._currencyPairsObjectPointer = currencyPairsObject;
        this._resultRenderPanel = document.createElement('div');
        this._errorRenderPanel = document.createElement('div');
        this._errorRenderPanel.classList.add("small")
        this._errorRenderPanel.classList.add("red");

        this._currencyPairsObjectPointer.listRenderElement.appendChild(this._resultRenderPanel);
        this._currencyPairsObjectPointer.listRenderElement.appendChild(this._errorRenderPanel);

        // 1st. iteration
        this.getPairs();

        // set automatic refresh
        setInterval(function() { me.getPairs() }, interval);
    }
    
    public getPairs() {
        var me = this;
        let pairsArray: Array<string> = me._currencyPairsObjectPointer.getPairIds;
        /* // we send always request for all currencies, because refresh is faster, when changing selector of currencies
        if (me._currencyFilter.length > 0) {
            // next request we sending with filter
            pairsArray = pairsArray.filter(function(n) {
                return me._currencyFilter.indexOf(n) !== -1;
            });
        }*/

        // refresh of last values
        me._currencyPairsObjectPointer.refreshLastValues(me._cachedResult);

        $.get({
            url: endpoint, 
            data: {currencyPairIds: pairsArray},
            dataType: "json"
        }).then(function(result) {
            me._cachedResult = result.rates;
            me.render();
            $(me._errorRenderPanel).hide();
            console.log(result);
        }).fail(function(err) {
            // if request failed
            me._errorRenderPanel.innerHTML = "Došlo k chybě při načítání, data nemusí odpovídat nejnovějším hodnotě.";
            $(me._errorRenderPanel).show();
        })
    }

    public render() {
        var me = this;
        me._resultRenderPanel.innerHTML = "";
        for (var item in me._cachedResult) {
            if (me._currencyPairsObjectPointer.currencyPairsSelection.length === 0 || (me._currencyPairsObjectPointer.currencyPairsSelection.length > 0 && me._currencyPairsObjectPointer.currencyPairsSelection.filter(x => x === item).length > 0)) {
                // we draw all or applying currency filter only
                me._resultRenderPanel.innerHTML += (me._currencyPairsObjectPointer.getCurrencyPair(item, me._cachedResult[item]));
            }
        }
    }
}
