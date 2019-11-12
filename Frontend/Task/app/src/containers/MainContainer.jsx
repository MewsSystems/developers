import React from 'react';
import {connect} from "react-redux";
import {
    fetchConfig,
    fetchCurrencyPairsValues, setFilter
} from "../actions/ExchangeRateActions";
import ConfigLoader from "../components/ConfigLoader";
import ExchangeRateUpdate from "../components/ExchangeRateUpdate";
import RatesList from "../components/RatesList";
import Filter from "../components/Filter";

const mapStateToProps = state => {
    return {
        config: state.config,
        currencyPairsRates: state.currencyPairsValues,
        filter: state.filter
    }
};

const mapDispatchToProps = dispatch => {
    return {
        fetchConfig: () => dispatch(fetchConfig()),
        fetchCurrencyPairsValues: codes => dispatch(fetchCurrencyPairsValues(codes)),
        onFilterChange: value => dispatch(setFilter(value))
    }
};

const MainContainer = (props) => {
    const {config, fetchConfig, fetchCurrencyPairsValues, filter, onFilterChange} = props;

    if (!config) {
        return <ConfigLoader onLoadConfig={fetchConfig}/>
    }

    return (
        <React.Fragment>
            <ExchangeRateUpdate fetchRates={fetchCurrencyPairsValues} pairIds={Object.keys(config)}/>
            <h1 align='center'>Exchange rates</h1>
            <Filter filter={filter} onFilterChange={onFilterChange}/>
            <RatesList {...props} />
        </React.Fragment>
    );
};

export default connect(mapStateToProps, mapDispatchToProps)(MainContainer);