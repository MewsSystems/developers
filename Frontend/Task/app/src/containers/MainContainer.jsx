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
import Grid from "@material-ui/core/Grid";

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
            <Grid container>
                <Grid item xs={10}>
                    <Filter config={config} filter={filter} onFilterChange={onFilterChange}/>
                </Grid>
                <Grid item xs={2}>
                    <ExchangeRateUpdate fetchRates={fetchCurrencyPairsValues} pairIds={Object.keys(config)}/>
                </Grid>
                <Grid item xs={12}>
                    <RatesList {...props} />
                </Grid>
            </Grid>
        </React.Fragment>
    );
};

export default connect(mapStateToProps, mapDispatchToProps)(MainContainer);