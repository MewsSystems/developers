"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var config_json_1 = require("../config.json");
var Rates_1 = require("./Rates");
var Settings_1 = require("./Settings");
var CurrencyPairs = /** @class */ (function () {
    function CurrencyPairs(_renderElement) {
        this.ratesObjectPointer = undefined;
        this._currencyPairs = {};
        this._lastRates = {};
        this.listRenderElement = document.createElement("div");
        this.settingsRenderElement = document.createElement("div");
        this.currencyPairsSelection = [];
        this._renderElement = document.getElementById(_renderElement.replace('#', ''));
        if (this._renderElement === undefined || this._renderElement === null) {
            alert("Došlo k chybě při inicializaci aplikace, chybí vykreslovací element.");
        }
        else {
            this._renderElement.innerHTML = "Starting app...";
            this.loadCurrencyPairs();
        }
    }
    CurrencyPairs.prototype.loadCurrencyPairs = function () {
        var dfd = $.Deferred();
        var me = this;
        // load from localstorage
        var savedData = localStorage.getItem("currencyPairsSelection");
        var savedCurrencyPairs;
        if (savedData !== undefined && savedData !== null) {
            savedCurrencyPairs = JSON.parse(savedData);
        }
        $.get(config_json_1.endpoint.replace("/rates", "") + "/configuration").then(function (result) {
            me._currencyPairs = result.currencyPairs;
            // we launch savedPairs or if not exists, then all loaded ids
            me.currencyPairsSelection = savedCurrencyPairs || me.getPairIds;
            // clcear element
            me._renderElement.innerHTML = "";
            me.createTabs();
            (new Settings_1.Settings(me)).render();
            me.ratesObjectPointer = new Rates_1.Rates(me);
            dfd.resolve();
        }, function (err, trace) {
            if (confirm("Aplikaci se nepodařilo inicializovat. Přejete si provést inicializaci znovu?")) {
                me.loadCurrencyPairs();
            }
            dfd.reject(err);
        });
        return dfd.promise();
    };
    CurrencyPairs.prototype.createTabs = function () {
        var me = this;
        var tabsContainer = document.createElement("div");
        tabsContainer.style.display = "flex";
        // tab settings
        var tabSettings = document.createElement("div");
        tabSettings.innerHTML = "Nastavení";
        tabSettings.classList.add("tab");
        tabSettings.onclick = function () {
            me.listRenderElement.style.display = "none";
            me.settingsRenderElement.style.display = "block";
            if (!tabSettings.classList.contains("selected")) {
                tabSettings.classList.add("selected");
            }
            if (tabList.classList.contains("selected")) {
                tabList.classList.remove("selected");
            }
            localStorage.setItem("currencyPairsActiveTab", "tabSettings");
        };
        tabsContainer.appendChild(tabSettings);
        // --
        // tab currencyList
        var tabList = document.createElement("div");
        tabList.innerHTML = "Kurzy";
        tabList.classList.add("tab");
        tabList.onclick = function () {
            me.listRenderElement.style.display = "block";
            me.settingsRenderElement.style.display = "none";
            if (!tabList.classList.contains("selected")) {
                tabList.classList.add("selected");
            }
            if (tabSettings.classList.contains("selected")) {
                tabSettings.classList.remove("selected");
            }
            localStorage.setItem("currencyPairsActiveTab", "tabList");
        };
        tabsContainer.appendChild(tabList);
        // --
        this._renderElement.appendChild(tabsContainer);
        this._renderElement.appendChild(this.listRenderElement);
        this._renderElement.appendChild(this.settingsRenderElement);
        // selecting tab
        var selectedTab = localStorage.getItem("currencyPairsActiveTab") || "tabSettings";
        if (selectedTab == "tabSettings") {
            tabSettings.classList.add("selected");
            $(this.listRenderElement).hide();
        }
        else {
            tabList.classList.add("selected");
            $(this.settingsRenderElement).hide();
        }
    };
    Object.defineProperty(CurrencyPairs.prototype, "getPairIds", {
        get: function () {
            return Object.keys(this._currencyPairs);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CurrencyPairs.prototype, "currencyPairs", {
        get: function () {
            return this._currencyPairs;
        },
        enumerable: true,
        configurable: true
    });
    CurrencyPairs.prototype.getCurrencyPair = function (id, newValue) {
        var tendency = "";
        if (this._lastRates[id] !== undefined) {
            // adding arrows of tendency to GUI
            tendency = this._lastRates[id] > newValue ? '<i class="fas fa-long-arrow-alt-down green"></i>' : (this._lastRates[id] < newValue ? '<i class="fas fa-long-arrow-alt-up red"></i>' : '<i class="fas fa-long-arrow-alt-right blue"></i>');
        }
        else {
            tendency = "unknown";
        }
        return "<div>" + this._currencyPairs[id][0].code + "/" + this._currencyPairs[id][1].code + ", Trend: " + tendency + " (" + newValue + ")</div>";
    };
    CurrencyPairs.prototype.refreshLastValues = function (newValues) {
        // refreshing cache of old values for tendency informations
        for (var item in newValues) {
            this._lastRates[item] = newValues[item];
        }
    };
    return CurrencyPairs;
}());
exports.CurrencyPairs = CurrencyPairs;
