// @flow
import React from 'react';
import { hot } from 'react-hot-loader';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { FetchCurrencyPairs, FetchCurrencyRates } from 'Utils/rates';
import { GetFilter, GetPairs } from 'Utils/localStorage';
import { interval } from 'Config/app-config.json';
import * as actionCreators from 'Redux/actions';
import isEqual from 'lodash.isequal';
import ExchangeListContainer from 'Containers/exchangeList';
import Header from 'Components/header';
import style from './app.scss';

type Props = {
  storePairs: Function,
  storeRates: Function,
  currencyPairs: Object,
  currencyRates: Object,
};

type State = {
  counter: number,
  filteredIdList: any,
};

class App extends React.Component<Props, State> {
  state = {
    counter: 0,
    filteredIdList: [],
  };

  componentWillMount() {
    const { storePairs, currencyPairs } = this.props;
    const filter = GetFilter() || '';
    const localPairs: any = GetPairs();

    if (localPairs && Object.keys(localPairs).length > 0) {
      storePairs(localPairs);

      this.matchExchange(filter, localPairs.currencyPairs);
    }
    FetchCurrencyPairs().then((res: any) => {
      if (!isEqual(res, localPairs)) {
        storePairs(res);
        this.matchExchange(filter, res.currencyPairs);
      }
    });
    this.rateUpdater(currencyPairs, interval);
  }

  rateUpdater = (data, time) => {
    this.refreshTimer();
    this.getRates();
    setInterval(() => {
      this.refreshTimer();
      this.getRates();
    }, time);
  };

  refreshTimer = () => {
    let counter = interval / 1000;
    const countdown = setInterval(() => {
      counter -= 1;
      this.setState({ counter });
      if (counter === 0) {
        clearInterval(countdown);
      }
    }, 1000);
  };

  getRates = () => {
    const { currencyPairs, storeRates } = this.props;

    const currencyPairIds = Object.entries(currencyPairs).map(pair => {
      const [key] = pair;
      return key;
    });

    const params = {
      currencyPairIds: [...currencyPairIds],
    };
    FetchCurrencyRates(params).then(res => {
      storeRates(res);
    });
  };

  matchExchange = (wordToMatch, currencyPairs) => {
    const regex = new RegExp(wordToMatch, 'i');
    let matchList: any = {};
    Object.keys(currencyPairs).map(key => {
      if (
        currencyPairs[key][0].code.match(regex) ||
        currencyPairs[key][1].code.match(regex) ||
        currencyPairs[key][0].name.match(regex) ||
        currencyPairs[key][1].name.match(regex)
      ) {
        matchList = [...matchList, key];
      }
      return null;
    });

    this.setState({ filteredIdList: matchList });
  };

  render() {
    const { counter, filteredIdList } = this.state;
    const { currencyPairs, currencyRates } = this.props;

    return (
      <div className={style.app}>
        <Header
          matchExchange={this.matchExchange}
          currencyPairs={currencyPairs}
          filter={GetFilter}
          counter={counter}
        />
        <ExchangeListContainer
          filteredIdList={filteredIdList}
          currencyPairs={currencyPairs}
          currencyRates={currencyRates}
        />
      </div>
    );
  }
}

function mapStateToProps(state) {
  return { currencyPairs: state.pairs, currencyRates: state.rates };
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators(actionCreators, dispatch);
}

export default hot(module)(
  connect(
    mapStateToProps,
    mapDispatchToProps,
  )(App),
);
