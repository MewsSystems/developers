import * as React from 'react';
import {connect} from 'react-redux';
import {ParsedCurrency, ParsedRate, RatesObject} from "../../../types/app";
import {RootState} from "../../../types/state";
import {compareRates, parseCurrencies, parseRates} from '../../common/helpers';
import {getConfig, getRates} from "../../actions/app";
import Filters from "../sections/Filters";
import RateGrid from "../sections/RateGrid";
import {start as startRateWorker, stop as stopRateWorker} from '../../workers/rate';

import './MainPage.scss';
import {addRate, removeRate} from "../../actions/user";

interface MainPageProps {
    loading?: boolean;
    userRates?: string[];
    currencies?: ParsedCurrency[];
    rates?: ParsedRate[];
    getConfig?: () => Promise<any>;
    getRates?: (rates: RatesObject) => void;
    addRate?: (id: string) => void;
    removeRate?: (id: string) => void;
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
            this.props.getRates(data);
        }, () => {
            console.log('error')
        });
    }

    public componentDidUpdate(prevProps: MainPageProps) {
        if (prevProps.userRates !== this.props.userRates) {
            stopRateWorker();

            if (this.props.userRates && this.props.userRates.length) {
                this.runWorker();
            }
        }

        if (prevProps.rates !== this.props.rates) {
            if (this.props.userRates && this.props.userRates.length) {
                this.setState({actualRates: compareRates(prevProps.rates, this.props.rates)});
            } else {
                this.setState({actualRates: []});
            }
        }
    }

    public async componentDidMount() {
        await this.props.getConfig();

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
                <RateGrid loading={this.props.loading} rates={this.state.actualRates}/>
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
        rates: parseRates(app.rates, currencies)
    };
};

const mapDispatchToProps = dispatch => {
    return {
        getConfig: () => dispatch(getConfig()),
        getRates: (rates: RatesObject) => dispatch(getRates(rates)),
        addRate: id => dispatch(addRate(id)),
        removeRate: id => dispatch(removeRate(id))
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(MainPage);