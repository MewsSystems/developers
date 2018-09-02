import React from 'react';
import PropTypes from 'prop-types';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { applySpec } from 'ramda';
import { isNilOrEmpty } from 'ramda-adjunct';
import LoadingModal from './loadingModal';
import Navbar from './navbar';
import CurrencyRates from '../CurrencyRates';
import { getCurrencyPairs as getCurrencyPairsAction, getRates, showCountdown } from '../../actions';
import {
    getCurrencyPairsById, getInterval, getShowCountdown, getTrackedCurrencyPairIds, isFetching,
} from '../../selectors';

class App extends React.Component {

    constructor(props) {
        super(props);
        const { currencyPairs, getCurrencyPairsAction } = props;
        if (isNilOrEmpty(currencyPairs)) {
            getCurrencyPairsAction();
        }
        this.state = {
            showLoadingModal: false,
        };
    }

    static getDerivedStateFromProps(nextProps, prevState) {
        if (nextProps.isFetchingCurrencyRates) {
            return { ...prevState, showLoadingModal: true };
        } else {
            return { ...prevState, showLoadingModal: false };
        }
    }

    componentDidMount() {
        this.timerId = setInterval(() => {
            const { trackedCurrencyPairIds, getRates, showCountdown, showCountdownFunc } = this.props;
            getRates(trackedCurrencyPairIds);
            if (!showCountdown) {
                showCountdownFunc();
            }
        }, this.props.interval);
    }

    componentDidUpdate(prevProps) {
        if (this.props.interval !== prevProps.interval) {
            clearInterval(this.timerId);
            this.timerId = setInterval(() => {
                const { trackedCurrencyPairIds, getRates } = this.props;
                getRates(trackedCurrencyPairIds);
            }, this.props.interval);
        }
    }

    componentWillUnmount() {
        clearInterval(this.timerId);
    }

    render() {
        return (
            <React.Fragment>
                <Navbar navbarBrand="Currency Rates Client" />
                <LoadingModal showLoadingModal={this.state.showLoadingModal} />
                { !this.props.isFetchingCurrencyRates && <CurrencyRates /> }
            </React.Fragment>
        );
    }
}

App.propTypes = {
    currencyPairs: PropTypes.any,
    getCurrencyPairsAction: PropTypes.func,
    getRates: PropTypes.func,
    interval: PropTypes.number,
    isFetchingCurrencyRates: PropTypes.bool,
    showCountdown: PropTypes.bool,
    showCountdownFunc: PropTypes.func,
    trackedCurrencyPairIds: PropTypes.arrayOf(PropTypes.string).isRequired,
};

const mapStateToProps = applySpec({
    currencyPairs: getCurrencyPairsById,
    interval: getInterval,
    isFetchingCurrencyRates: isFetching('currencyPairs'),
    showCountdown: getShowCountdown,
    trackedCurrencyPairIds: getTrackedCurrencyPairIds,
});

const mapDispatchToProps = (dispatch) => bindActionCreators({
    getCurrencyPairsAction,
    getRates,
    showCountdownFunc: showCountdown,
}, dispatch);

export default connect(mapStateToProps, mapDispatchToProps)(App);
