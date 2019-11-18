import React, { Component } from 'react';
import Currency from './models/currency';

class App extends Component {
  componentDidMount() {
   this.loadData();
  }

  async loadData() {
    try {
      const data = await Currency.loadConfiguration();

    } catch (e) {
      alert(e);
    }
  };

  render() {
    return (
      <div>
        APP
      </div>
    );
  }
}
export default App;