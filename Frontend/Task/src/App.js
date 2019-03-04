import React, { Component } from 'react';
import './App.css';
import Rates from './features/Rates';

class App extends Component {
  render() {
    return (
      <div className="App">
        <header className="App-header">
          <Rates />
        </header>
      </div>
    );
  }
}

export default App;
