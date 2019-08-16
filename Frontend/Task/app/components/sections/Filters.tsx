import * as React from 'react';
import {ParsedCurrency} from "../../../types/app";

import './Filters.scss';

interface FiltersProps {
    loading: boolean;
    currencies: ParsedCurrency[];
    userRates: string[];
    addRate: (id: string) => void;
    removeRate: (id: string) => void;
}

interface FiltersState {
    userRate: string;
}

export default class Filters extends React.Component<FiltersProps, FiltersState> {
    private onUserRateChange(event: any) {
        this.props.addRate(event.target.value);
        this.setState({userRate: ''});
    }

    private removeFilter(id: string) {
        return () => {
            this.props.removeRate(id);
        };
    }

    constructor(props) {
        super(props);

        this.state = {
            userRate: ''
        };

        this.renderContent = this.renderContent.bind(this);
        this.renderOptions = this.renderOptions.bind(this);
        this.onUserRateChange = this.onUserRateChange.bind(this);
        this.removeFilter = this.removeFilter.bind(this);
    }

    public render() {
        return (
            <section className="filter-section">
                <div className="card">
                    <div className="card-body">
                        {this.props.loading ? this.renderLoading() : this.renderContent()}
                    </div>
                </div>
            </section>
        );
    }

    public renderContent() {
        return (
            <div className="content">
                <select className="form-control" value={this.state.userRate} onChange={this.onUserRateChange}>
                    <option value="">--- Select Currency Pair ---</option>
                    {this.renderOptions()}
                </select>
                {this.renderSelectedFilters()}
            </div>
        );
    }

    public renderOptions() {
        return this.props.currencies
            .filter(currency => this.props.userRates.indexOf(currency.id) === -1)
            .map(currency => {
                return (
                    <option key={currency.id}
                            value={currency.id}>{currency.currency1.name} / {currency.currency2.name}</option>
                );
            });
    }

    public renderSelectedFilters() {
        const currencies = this.props.currencies.filter(currency => this.props.userRates.indexOf(currency.id) !== -1);

        return (
            <div className="filter-list">
                {
                    currencies.map(currency => {
                        return (
                            <div key={`${currency.id}-filter`} className="filter-entry">
                                <span>{currency.currency1.code} / {currency.currency2.code}</span>
                                <i className="fas fa-times" onClick={this.removeFilter(currency.id)}/>
                            </div>
                        )
                    })
                }
            </div>
        );
    }

    public renderLoading() {
        return (
            <i className="fas fa-spinner fa-spin"/>
        );
    }
}