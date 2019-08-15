import * as React from 'react';
import {connect} from 'react-redux';
import {ParsedCurrency, ParsedRate} from "../../../types/app";
import {RootState} from "../../../types/state";
import {compareRates, parseCurrencies, parseRates} from '../../common/helpers';

import './MainPage.scss';
import {getConfig} from "../../actions/app";
import Filters from "../sections/Filters";

interface MainPageProps {
    loading?: boolean;
    userRates?: string[];
    currencies?: ParsedCurrency[];
    rates?: ParsedRate[];
    getConfig?: () => Promise<any>;
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

    public componentDidUpdate(prevProps: MainPageProps) {
        if (prevProps.rates !== this.props.rates) {
            this.setState({actualRates: compareRates(prevProps.rates, this.props.rates)});
        }
    }

    public async componentDidMount() {
        await this.props.getConfig();
    }

    public render() {
        return (
            <main>
                <Filters loading={this.props.loading} currencies={this.props.currencies}/>
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
        getConfig: () => dispatch(getConfig())
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(MainPage);