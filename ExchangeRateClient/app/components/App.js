import React, { Component } from 'react';
import { endpoint } from '../config';
// redux
import { connect } from 'react-redux';
import { setCurrencyPairs } from '../store/actions';
// components
import CurrencyPair from './CurrencyPair';

let rates;

class App extends Component {
  constructor(props) {
      super(props);
  }

  componentWillMount() {
    if(localStorage.getItem('currencyPairs')) {
      this.props.setCurrencyPairs(JSON.parse(localStorage.getItem('currencyPairs')));
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
        localStorage.setItem('currencyPairs', JSON.stringify(data.currencyPairs));
    })
    .catch(error => console.log(error.message));
  }

  render() {
    const { currencyPairs } = this.props;
    const cards = {
      display: 'flex',
      flexFlow: 'row wrap',
    }

    console.log(currencyPairs)

    return (
      <div style={cards}>    
        {
          (currencyPairs !== undefined) 
          ? Object.keys(currencyPairs).map(pair => <CurrencyPair key={pair} pair={pair} />)
          : <p>Loading...</p>
        }
      </div>
    )   
  }
}

const mapStateToProps = state => ({ 
  currencyPairs: state.currencyPairs,
});

const mapDispatchToProps = dispatch => ({
  setCurrencyPairs: pairs => dispatch(setCurrencyPairs(pairs)),
});

export default connect(mapStateToProps, mapDispatchToProps)(App)