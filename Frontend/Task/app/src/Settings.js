"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Settings = /** @class */ (function () {
    function Settings(currencyPairsObject) {
        this._currencyPairsObjectPointer = currencyPairsObject;
    }
    Settings.prototype.render = function () {
        var me = this;
        var _loop_1 = function () {
            var input = document.createElement("input");
            input.type = "checkbox";
            input.id = "checkbox_" + item;
            var label = document.createElement("label");
            label.for = "checkbox_" + item;
            var pair = me._currencyPairsObjectPointer.currencyPairs[item];
            label.innerHTML = pair[0].code + "/" + pair[1].code;
            label.title = pair[0].name + " / " + pair[1].name;
            label.style.cursor = "help";
            // adding elements
            this_1._currencyPairsObjectPointer.settingsRenderElement.appendChild(input);
            this_1._currencyPairsObjectPointer.settingsRenderElement.appendChild(label);
            this_1._currencyPairsObjectPointer.settingsRenderElement.appendChild(document.createElement("div"));
            // --
            var currencyPairId = item;
            input.onchange = function (e) {
                //always remove
                me._currencyPairsObjectPointer.currencyPairsSelection = me._currencyPairsObjectPointer.currencyPairsSelection.filter(function (x) { return x !== currencyPairId; });
                if (this.checked) {
                    // add - remove before for deduplication
                    me._currencyPairsObjectPointer.currencyPairsSelection.push(currencyPairId);
                }
                // persist save
                localStorage.setItem("currencyPairsSelection", JSON.stringify(me._currencyPairsObjectPointer.currencyPairsSelection));
                // refresh courses - Rates object will be always defined
                me._currencyPairsObjectPointer.ratesObjectPointer.render();
            };
            // if is in selection
            if (me._currencyPairsObjectPointer.currencyPairsSelection.filter(function (x) { return x === currencyPairId; }).length > 0) {
                input.checked = true;
            }
        };
        var this_1 = this;
        for (var item in me._currencyPairsObjectPointer.currencyPairs) {
            _loop_1();
        }
    };
    return Settings;
}());
exports.Settings = Settings;
