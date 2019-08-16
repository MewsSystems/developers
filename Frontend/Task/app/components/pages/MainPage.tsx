import * as React from 'react';
import {connect} from 'react-redux';
import {ParsedCurrency, ParsedRate, RatesObject} from "../../../types/app";
import {RootState} from "../../../types/state";
import {compareRates, parseCurrencies, parseRates} from '../../common/helpers';
import {clearRates, getConfig, getRates, setError, setRateCallTime} from "../../actions/app";
import Filters from "../sections/Filters";
import RateGrid from "../sections/RateGrid";
import {start as startRateWorker, stop as stopRateWorker} from '../../workers/rate';
import {addRate, removeRate} from "../../actions/user";
import * as moment from 'moment';

import './MainPage.scss';

interface MainPageProps {
    loading?: boolean;
    userRates?: string[];
    currencies?: ParsedCurrency[];
    rates?: ParsedRate[];
    rateRequestDate?: string;
    error?: boolean;
    getConfig?: () => Promise<any>;
    getRates?: (rates: RatesObject) => void;
    addRate?: (id: string) => void;
    removeRate?: (id: string) => void;
    clearRates?: () => void;
    setError?: (status: boolean) => void;
    setRateCallTime?: (time: string) => void;
}

interface MainPageState {
    actualRates: ParsedRate[];
}

class MainPage extends React.Component<MainPageProps, MainPageState> {
    constructor(props) {
        super(props);

        this.state = {
            actualRates: []
        };
    }

    private runWorker() {
        startRateWorker(this.props.userRates, data => {
            this.setState({actualRates: compareRates(this.props.rates, parseRates(data, this.props.currencies))});

            this.props.getRates(data);
            this.props.setRateCallTime(moment().toISOString());
            this.props.setError(false);
        }, () => {
            // Added to immediately remove an element from grid in the case if corresponding request has failed
            this.setState({actualRates: this.state.actualRates.filter(rate => this.props.userRates.indexOf(rate.id) > -1)});
            this.props.setError(true);
        });
    }

    public componentDidUpdate(prevProps: MainPageProps) {
        if (prevProps.userRates !== this.props.userRates) {
            stopRateWorker();

            if (this.props.userRates && this.props.userRates.length) {
                this.runWorker();
            } else {
                this.setState({actualRates: []});
                this.props.clearRates();
            }
        }
    }

    public async componentDidMount() {
        if (!this.props.currencies || !this.props.currencies.length) {
            await this.props.getConfig();
        }

        if (this.props.userRates && this.props.userRates.length) {
            this.runWorker();
        }
    }

    public render() {
        return (
            <main>
                <Filters loading={this.props.loading} userRates={this.props.userRates}
                         currencies={this.props.currencies}
                         addRate={this.props.addRate}
                         removeRate={this.props.removeRate}/>
                <RateGrid loading={this.props.loading}
                          rates={this.state.actualRates}
                          date={this.props.rateRequestDate}
                          error={this.props.error}/>
            </main>
        );
    }
}

const mapStateToProps = ({app, user}: RootState) => {
    const currencies = parseCurrencies(app.currencies);

    return {
        loading: app.loading,
        userRates: user.userRates,
        currencies,
        rates: parseRates(app.rates, currencies),
        rateRequestDate: app.date,
        error: app.error
    };
};

const mapDispatchToProps = dispatch => {
    return {
        getConfig: () => dispatch(getConfig()),
        getRates: (rates: RatesObject) => dispatch(getRates(rates)),
        addRate: id => dispatch(addRate(id)),
        removeRate: id => dispatch(removeRate(id)),
        clearRates: () => dispatch(clearRates()),
        setError: (status: boolean) => dispatch(setError(status)),
        setRateCallTime: (time: string) => dispatch(setRateCallTime(time))
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(MainPage);