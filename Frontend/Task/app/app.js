import React, { Component } from 'react';

import Currency from './models/currency';

import PairsSelect from './components/PairsSelect';
import RateList from './components/RateList';

import config from './config';

import styles from './style.scss';

class App extends Component {

  constructor() {
    super();

    this.updateState = this.updateState.bind(this);
  }

  state = {
    selectValue: [],
    rates: {},
    update: false,
    error: false,
    countdownValue: config.interval / 1000,
  };

  interval = null;
  countdown = null;

  componentDidUpdate(prevProps, prevState): void {
    if (this.state.selectValue.length !== prevState.selectValue.length) {
      /** Clear interval of the API update **/
      if (this.interval) {
        clearInterval(this.interval);
      }

      /** Clear interval of countdown **/
      if (this.countdown) {
        clearInterval(this.countdown);
      }

      this.loadRates();
      this.interval = setInterval(() => this.loadRates(), config.interval);
      this.countdown = setInterval(() => this.setState({ countdownValue: this.state.countdownValue - 1 }), 1000);
    }
  }

  componentWillUnmount() {
    if (this.interval) {
      clearInterval(this.interval);
    }
  }

  /**
   * Load rates from server
   * @return {Promise<void>}
   */
  async loadRates() {
    const { selectValue } = this.state;

    const dataToSend = {
      currencyPairIds: selectValue,
    };

    try {
      const { rates } = await Currency.loadRates(dataToSend);

      this.setState({ rates, update: true, error: false, countdownValue: config.interval / 1000 });
    } catch (e) {
      this.setState({ error: true, countdownValue: config.interval / 1000 })
    }
  }

  /**
   * Render message component
   * @return {*}
   */
  serverMessage() {
    const { error } = this.state;

    return (
      <div className={styles.messageContainer}>
        {!error && <span className={styles.noError}>Server is responding</span>}
        {error && <span className={styles.error}>Server is not responding</span>}
        <span>Next update in {this.state.countdownValue} s...</span>
      </div>
    )
  }

  /**
   * Update state
   * @param {Object} data
   */
  updateState(data) {
    const { name, value } = data;

    this.setState({ [name]: value });
  }

  render() {
    const { rates, selectValue, update } = this.state;

    return (
      <div className={styles.container}>
        <div className={styles.wrap}>
          <PairsSelect
            value={selectValue}
            updateState={this.updateState}
          />
          <RateList
            currencyKeys={selectValue}
            rates={rates}
            update={update}
            setUpdate={() => this.setState({ update: false })}
          />
          {this.serverMessage()}
        </div>
      </div>
    );
  }
}

export default App;