import React, { Component } from 'react';
import './App.css';
import Rates from './features/Rates';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

class App extends Component {
  render() {
    const {
      config: { interval },
    } = this.props;

    return (
      <div className="App">
        <header className="App-header">
          <Rates updateInterval={interval} />
        </header>
        <ToastContainer autoClose={interval} />
      </div>
    );
  }
}

export default App;
