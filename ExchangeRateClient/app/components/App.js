import React, { Component } from 'react';
import { endpoint } from '../config';
// redux
import { connect } from 'react-redux';
import { setCurrencyPairs, setTogglePairs } from '../store/actions';
// components
import CurrencyPair from './CurrencyPair';
import CurrencySelector from './CurrencySelector';

class App extends Component {
  constructor(props) {
      super(props);
  }

  componentWillMount() {
    if(localStorage.getItem('currencyPairs')) {
      this.props.setCurrencyPairs(JSON.parse(localStorage.getItem('currencyPairs')));
    }
    if(localStorage.getItem('togglePairs')) {
      this.props.setTogglePairs(JSON.parse(localStorage.getItem('togglePairs')));
    }
  }
  
  componentDidMount() {
    if(!localStorage.getItem('currencyPairs')) {
      this.getPairs();
    } 
  };

  getPairs() {
    fetch(endpoint + "/configuration")
    .then(response => response.json())
    .then(data => {
        this.props.setCurrencyPairs(data.currencyPairs);
        const togglePairs = {};
        
        Object.keys(data.currencyPairs).map(key => togglePairs[key] = true);

        this.props.setTogglePairs(togglePairs)
        
        localStorage.setItem('currencyPairs', JSON.stringify(data.currencyPairs));
        localStorage.setItem('togglePairs', JSON.stringify(togglePairs))
    })
    .catch(error => console.log(error.message));
  }

  render() {
    const { currencyPairs, togglePairs } = this.props;
    const cards = {
      display: 'flex',
      flexFlow: 'row wrap',
    }

    return (
      (currencyPairs !== undefined && togglePairs !== undefined) 
      ? <div>
          {<CurrencySelector />}
          <div style={cards}>
            {Object.keys(currencyPairs).map(pair => togglePairs[pair] && <CurrencyPair key={pair} pair={pair} />)}
          </div>
        </div>
      : <p>Loading...</p>
    )   
  }
}

const mapStateToProps = state => ({ 
  currencyPairs: state.currencyPairs,
  togglePairs: state.togglePairs
});

const mapDispatchToProps = dispatch => ({
  setCurrencyPairs: pairs => dispatch(setCurrencyPairs(pairs)),
  setTogglePairs: togglePairs => dispatch(setTogglePairs(togglePairs))
});

export default connect(mapStateToProps, mapDispatchToProps)(App)