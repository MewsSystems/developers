import React, { Component } from 'react';
import EventAggregator from '../EventAggregator';
import EEventMessages from '../abstracts/EEventMessages';
import MenuState from '../models/MenuState';

export class Menu extends Component {
    private _events: EventAggregator = EventAggregator.instance as EventAggregator;
    public state: MenuState = new MenuState();
    private _appInitialized: boolean = false;

    componentWillMount() {
        this._events.subscribe(this.notifyAction.bind(this));
    }

    private notifyAction(message: EEventMessages, value: any) {
        if (message === EEventMessages.ConfigurationLoaded) {
            this._appInitialized = true;
        }
        if (message === EEventMessages.MenuTabSelected) {
            let data: { tab: string } = value;
            var newState = new MenuState();
            newState.activeTab = data.tab;
            this.setState(newState);
        }
    }

    private selectSettings(): any {
        var data = { tab: "settings" };
        this._events.notify(EEventMessages.MenuTabSelected, data);
    }
    private selectRates(): any {
        var data = { tab: "rates" };
        this._events.notify(EEventMessages.MenuTabSelected, data);
    }

    private buttonSettings() {
        var action = this.selectSettings.bind(this);
        return <div className={this.state.classNamesSettings} onClick={action}>Nastavení</div>
    }
    private buttonRates() {
        var action = this.selectRates.bind(this);
        return <div className={this.state.classNamesRates} onClick={action}>Kurzy</div>
    }

    public render() {
        if (!this._appInitialized) {
            return <div>Načítání aplikace ...</div>;
        } else {
            return <div className="appMenuBar">{this.buttonSettings()} {this.buttonRates()}</div>
        }
    }
}