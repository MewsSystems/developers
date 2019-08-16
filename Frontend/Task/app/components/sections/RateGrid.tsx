import * as React from 'react';
import {ParsedRate} from "../../../types/app";

import './RateGrid.scss';

interface RateGridProps {
    loading: boolean;
    rates: ParsedRate[];
}

export default class RateGrid extends React.Component<RateGridProps, any> {
    constructor(props) {
        super(props);

        this.renderRows = this.renderRows.bind(this);
    }

    public render() {
        return (
            <section className="rate-grid-section">
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
                {this.props.rates && this.props.rates.length ? this.renderEntries() : this.renderNoEntries()}
            </div>
        );
    }

    public renderEntries() {
        return (
            <table className="table table-striped table-sm">
                <thead>
                <tr>
                    <th>Name</th>
                    <th>Value</th>
                    <th>Type</th>
                </tr>
                </thead>
                <tbody>
                {this.renderRows()}
                </tbody>
            </table>
        );
    }

    public renderRows() {
        return this.props.rates.map(rate => {
            return (
                <tr key={rate.id}>
                    <td>{rate.name}</td>
                    <td>{rate.value}</td>
                    <td>{rate.type ? rate.type.toUpperCase() : '-'}</td>
                </tr>
            );
        });
    }

    public renderNoEntries() {
        return (
            <div className="no-entries">
                <i className="fas fa-times-circle"/>
                <b>No entries available. Please, select at least one filter.</b>
            </div>
        );
    }

    public renderLoading() {
        return (
            <i className="fas fa-spinner fa-spin"/>
        );
    }
}