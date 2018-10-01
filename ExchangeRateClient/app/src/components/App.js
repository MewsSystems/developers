import React, { Component } from 'react';

import Navbar from './layout/Navbar';
import Footer from './layout/Footer';
import Main from './layout/Landing';
class App extends Component {
  render() {
    return (
      <div className="App">
        <Navbar />
        <div className="wrappar">
          <Main />
        </div>
      </div>
    );
  }
}
export default App;
