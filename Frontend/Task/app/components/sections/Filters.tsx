import * as React from 'react';
import {ParsedCurrency} from "../../../types/app";

import './Filters.scss';

interface FiltersProps {
    loading: boolean;
    currencies: ParsedCurrency[];
}

export default class Filters extends React.Component<FiltersProps, any> {
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
            <div/>
        );
    }

    public renderLoading() {
        return (
            <i className="fas fa-spinner fa-spin"/>
        );
    }
}