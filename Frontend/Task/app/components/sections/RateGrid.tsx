import * as React from 'react';
import {ParsedRate, RateType} from "../../../types/app";
import * as moment from 'moment';

import './RateGrid.scss';

interface RateGridProps {
    loading: boolean;
    rates: ParsedRate[];
    date: string;
    error: boolean;
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

    private renderContent() {
        return (
            <div className="content">
                {this.props.date && this.renderTimeHeader()}
                {this.props.rates && this.props.rates.length ? this.renderEntries() : this.renderNoEntries()}
            </div>
        );
    }

    private renderTimeHeader() {
        return (
            <div className="time-header">
                {this.props.error && (<i title="Something went wrong with last request, please wait"
                                         className="fas fa-exclamation-triangle"/>)}
                <span>Last successful request: {moment(this.props.date).format('HH:mm:ss DD/MM/YYYY')}</span>
            </div>
        );
    }

    private renderEntries() {
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

    private renderRows() {
        return this.props.rates.map(rate => {
            return (
                <tr key={rate.id}>
                    <td>{rate.name}</td>
                    <td>{rate.value}</td>
                    <td className="icon-column">{rate.type ? rate.type.toUpperCase() : '-'} {this.renderTrendIcon(rate.type)}</td>
                </tr>
            );
        });
    }

    private renderTrendIcon(type: RateType) {
        switch (type) {
            case RateType.growing:
                return <i className="fas fa-arrow-up"/>;
            case RateType.declining:
                return <i className="fas fa-arrow-down"/>;
            case RateType.stagnating:
                return <i className="fas fa-grip-lines"/>;
        }

        return null;
    }

    private renderNoEntries() {
        return (
            <div className="no-entries">
                <i className="fas fa-times-circle"/>
                <b>No entries available. Please, select at least one filter.</b>
            </div>
        );
    }

    private renderLoading() {
        return (
            <i className="fas fa-spinner fa-spin"/>
        );
    }
}