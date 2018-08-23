// @flow
import React from 'react';
import { hot } from 'react-hot-loader';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { FetchCurrencyPairs, FetchCurrencyRates } from 'Utils/rates';
import { interval } from 'Config/app-config.json';
import * as actionCreators from 'Redux/actions';

import style from './app.scss';

type Props = {
  storePairs: Function,
  storeRates: Function,
  currencyPairs: Object,
  currencyRates: Object,
};

type State = {
  counter: number,
};

class App extends React.Component<Props, State> {
  state = {
    counter: 0,
  };

  componentWillMount() {
    const { storePairs } = this.props;

    FetchCurrencyPairs().then(res => {
      storePairs(res);
      this.refreshTimer();
      this.getRates();

      setInterval(() => {
        this.refreshTimer();
        this.getRates();
      }, interval);
    });
  }

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

  render() {
    const { currencyPairs, currencyRates } = this.props;
    const { counter } = this.state;

    return (
      <div className={style.app}>
        <div className={style.countdown}>
          <div>{counter}</div>
        </div>
        <ul>
          {Object.keys(currencyPairs).map(key => (
            <li key={key}>
              ({currencyRates[key]}) {currencyPairs[key][0].code} /{' '}
              {currencyPairs[key][1].code}
            </li>
          ))}
        </ul>
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
