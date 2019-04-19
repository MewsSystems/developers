import { endpoint, interval } from '../config.json';
import { Rates } from './Rates';
import { Settings } from './Settings';
import ApiCall from './modules/ApiCall';
import AppCore from './AppCore';

export class CurrencyPairs {
    public ratesObjectPointer?: Rates = undefined;
    
    private _currencyPairs: { [s: string]: [ICurrecy, ICurrecy]; } = {};
    private _lastRates: { [s: string]: number; } = {};
    private _renderElement: HTMLDivElement;
    public listRenderElement: HTMLDivElement = document.createElement("div");
    public settingsRenderElement: HTMLDivElement = document.createElement("div");

    public currencyPairsSelection: Array<string> = [];
    
    constructor(_renderElement: string) {
        this._renderElement = <HTMLDivElement>document.getElementById(_renderElement.replace('#', ''));
        if (this._renderElement === undefined || this._renderElement === null) {
            alert("Došlo k chybě při inicializaci aplikace, chybí vykreslovací element.");
        } else {
            this._renderElement.innerHTML = "Starting app...";
            this.loadCurrencyPairs();
        }
    }

    public async loadCurrencyPairs() {
        var me = this;
        // load from localstorage
        var savedData = localStorage.getItem("currencyPairsSelection");
        var savedCurrencyPairs: Array<string> = [];
        if (savedData !== undefined && savedData !== null) {
            savedCurrencyPairs = JSON.parse(savedData);
        }

        var ac = new ApiCall();
        ac.endpoint = (<AppCore>AppCore.instance).endpointUrl;
        ac.method = "configuration";
        let result = await ac.sendRequest<ICurrencyPairsResponse>().catch(function(err) {
            if (confirm("Aplikaci se nepodařilo inicializovat. Přejete si provést inicializaci znovu?")) {
                me.loadCurrencyPairs();
            }
        });

        me._currencyPairs = (<any>result).currencyPairs;
            
        // we launch savedPairs or if not exists, then all loaded ids
        me.currencyPairsSelection = savedCurrencyPairs || me.getPairIds;
        
        // clcear element
        me._renderElement.innerHTML = "";
        me.createTabs(); 
        
        (new Settings(me)).render();
        me.ratesObjectPointer = new Rates(me);
        console.log(result);
        
        return result;
    }

    public createTabs() {
        var me = this;
        var tabsContainer = document.createElement("div");
        tabsContainer.style.display = "flex";

        // tab settings
        let tabSettings = document.createElement("div");
        tabSettings.innerHTML = "Nastavení";
        tabSettings.classList.add("tab");
        tabSettings.onclick = function() {
            me.listRenderElement.style.display = "none";
            me.settingsRenderElement.style.display = "block";
            if (!tabSettings.classList.contains("selected")) {
                tabSettings.classList.add("selected");
            }
            if (tabList.classList.contains("selected")) {
                tabList.classList.remove("selected");
            }
            localStorage.setItem("currencyPairsActiveTab", "tabSettings");
        }
        
        tabsContainer.appendChild(tabSettings);
        // --

        // tab currencyList
        let tabList = document.createElement("div");
        tabList.innerHTML = "Kurzy";
        tabList.classList.add("tab");
        tabList.onclick = function() {
            me.listRenderElement.style.display = "block";
            me.settingsRenderElement.style.display = "none";
            if (!tabList.classList.contains("selected")) {
                tabList.classList.add("selected");
            }
            if (tabSettings.classList.contains("selected")) {
                tabSettings.classList.remove("selected");
            }
            localStorage.setItem("currencyPairsActiveTab", "tabList");
        }
        
        tabsContainer.appendChild(tabList);
        // --
        
        this._renderElement.appendChild(tabsContainer);
        this._renderElement.appendChild(this.listRenderElement);
        this._renderElement.appendChild(this.settingsRenderElement);
        
        // selecting tab
        var selectedTab = localStorage.getItem("currencyPairsActiveTab") || "tabSettings";

        if (selectedTab == "tabSettings") {
            tabSettings.classList.add("selected");
            this.listRenderElement.style.display = "block";
        } else {
            tabList.classList.add("selected");
            this.settingsRenderElement.style.display = "none";
        }
    }

    public get getPairIds(): Array<string> {
        return Object.keys(this._currencyPairs);
    }

    public get currencyPairs() {
        return this._currencyPairs;
    }

    public getCurrencyPair(id: string, newValue: number) {
        var tendency = "";
        if (this._lastRates[id] !== undefined) {
            // adding arrows of tendency to GUI
            tendency = this._lastRates[id] > newValue ? '<i class="fas fa-long-arrow-alt-down green"></i>' : (this._lastRates[id] < newValue ? '<i class="fas fa-long-arrow-alt-up red"></i>' : '<i class="fas fa-long-arrow-alt-right blue"></i>');
        } else {
            tendency = "unknown";
        }

        return "<div>" + this._currencyPairs[id][0].code + "/" + this._currencyPairs[id][1].code + ", Trend: " + tendency + " (" + newValue +")</div>";
    }

    public refreshLastValues(newValues: { [s: string]: number; }) {
        // refreshing cache of old values for tendency informations
        for (var item in newValues) {
            this._lastRates[item] = newValues[item];
        }
    }
}

interface ICurrencyPairsResponse {
    currencyPairs: { [key: string] : [ICurrecy, ICurrecy] }
}

interface ICurrecy {
    code: string,
    name: string
}