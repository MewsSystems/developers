import React from 'react';
import {connect} from "react-redux";
import {fetchCurrencyPairsConfig, fetchCurrencyPairValue} from "../actions/ExchangeRateActions";

const mapStateToProps = state => {
    return {
        currencyPairsConfig: state.currencyPairsConfig
    }
};

const mapDispatchToProps = dispatch => {
    return {
        fetchCurrencyPairsConfig: () => dispatch(fetchCurrencyPairsConfig()),
        fetchCurrencyPairValue: code => fetchCurrencyPairValue(code)
    }
};

const MainContainer = (props) => {
    const {currencyPairsConfig, fetchCurrencyPairsConfig, fetchCurrencyPairValue} = props;

    const renderPairs = Object.keys(currencyPairsConfig).map(key => {
            return (
                <li key={key} onClick={() => fetchCurrencyPairValue(key)}>
                    {currencyPairsConfig[key][0].code} / {currencyPairsConfig[key][1].code}
                </li>
            )
        }
    );

    return (
        <div>
            Ahoj temný světe
            <button onClick={fetchCurrencyPairsConfig}>
                Stahni config
            </button>
            <ul>
                {renderPairs}
            </ul>
        </div>
    );
};

export default connect(mapStateToProps, mapDispatchToProps)(MainContainer);