import React, { PureComponent } from 'react';
import './App.scss';

import RatesList from '../RatesList';
import PairsSelector from '../PairsSelector'

class App extends PureComponent {
  render() {
    return (
      <div className="react-root">
        <PairsSelector/>
        {/*<RatesList/>*/}
      </div>
    );
  }
}

export default App;


