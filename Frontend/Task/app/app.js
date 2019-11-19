import React, { Component } from 'react';

import PairsSelect from './components/PairsSelect';
import Currency from './models/currency';

import config from './config';

class App extends Component {

  constructor() {
    super();

    this.updateState = this.updateState.bind(this);
  }

  state = {
    selectValue: [],
  };

  interval = null;

  componentDidUpdate(prevProps, prevState): void {
    if (this.state.selectValue || this.state.selectValue !== prevState.selectValue) {
      // if (this.interval) {
      //   clearInterval(this.interval);
      // }

      this.interval = setInterval(() => this.loadRates(), config.interval);
    }
  }

  componentWillUnmount() {
    if (this.interval) {
      clearInterval(this.interval);
    }
  }

  async loadRates() {
    const { selectValue } = this.state;

    const dataToSend = {
      currencyPairIds: selectValue,
    };

    try {
      const rates = await Currency.loadRates(dataToSend);

    } catch (e) {
      alert(e);
    }
  }

  updateState(data) {
    const { name, value } = data;

    this.setState({ [name]: value });
  }

  render() {
    return (
      <div>
        <PairsSelect
          value={this.state.selectValue}
          updateState={this.updateState}
        />
      </div>
    );
  }
}

export default App;