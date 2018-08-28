import React, { Component } from 'react';
import { endpoint, interval } from '../config';
// redux
import { connect } from 'react-redux';
import { setCurrencyPairs, setRates, selectPair } from '../store/actions';
// components
import PairSelector from './PairSelector';

let rates;

class App extends Component {
  constructor(props) {
      super(props);

      this.state = {
        trend: "--"
      }

      this.getRates = this.getRates.bind(this);
      this.getTrend = this.getTrend.bind(this);
      this.handleChange = this.handleChange.bind(this);
  }

  componentWillMount() {
    if(localStorage.getItem('currencyPairs')) {
      const cp = JSON.parse(localStorage.getItem('currencyPairs'));
      const pair = Object.keys(cp)[0];

      this.props.setCurrencyPairs(cp);
      this.props.selectPair(pair);
      this.getRates(pair);
    }
  }
  
  componentDidMount() {
    const cp = localStorage.getItem('currencyPairs');

    if(!cp) {
      this.getPairs();
    } else {
      this.setInterval(Object.keys(JSON.parse(cp))[0])
    }   
  };

  getPairs() {
    fetch(endpoint + "/configuration")
    .then(response => response.json())
    .then(data => {
        let pair = Object.keys(data.currencyPairs)[0];

        this.props.setCurrencyPairs(data.currencyPairs);
        this.props.selectPair(pair);
        this.getRates(pair);
        this.setInterval(pair);

        localStorage.setItem('currencyPairs', JSON.stringify(data.currencyPairs));
    })
    .catch(error => console.log(error.message));
  }

  getRates(selected) {
    if(selected !== undefined) {
      fetch(endpoint + `/rates?currencyPairIds[]=${selected}`)
      .then(response => response.json())
      .then(data => {
        this.props.setRates(data.rates);
        this.props.selectPair(selected);
        this.getTrend(data.rates[selected]);
      })
      .catch(error => console.log(error.message));
    }
  }

  setInterval(selected) {
    return rates = setInterval(() => this.getRates(selected), interval);
  }

  getTrend(rate) {
    if(!localStorage.getItem('rate')) {
      localStorage.setItem('rate', JSON.stringify(rate));

      this.setState({ trend: "stagnating" });
      return;
    } 

    const prev = JSON.parse(localStorage.getItem('rate'));
    const next = rate;

    if(prev < next) {
      this.setState({ trend: "growing" });
    }
    if(prev > next) {
      this.setState({ trend: "declining" });
    }
    if(prev == next) {
      this.setState({ trend: "stagnating" });
    }
    
    localStorage.setItem('rate', next);
  }

  handleChange(event) {
    let selected = event.target.value;
    this.getRates(selected);

    clearInterval(rates);
    this.setInterval(selected);
  }

  render() {
      const { currencyPairs, rates, selected } = this.props;
      const alignCenter = { textAlign: 'center' };
      const card = {
        minWidth: '333px',
        display: 'inline-block',
        border: '1px solid #ddd',
        borderRadius: '5px',
        padding: '10px'
      };
      const mb = { marginBottom: '10px' };

      return (
        <div style={alignCenter}>
          <div style={card}>
          {
            (currencyPairs !== undefined) 
            ? <div>
                <PairSelector pairs={currencyPairs} onChange={this.handleChange} style={mb} />
                <div>{(selected !== undefined && rates !== undefined) ? rates[selected] : "--"}</div>
                <div>{(this.state.trend !== null) && this.state.trend}</div>
              </div>
            : <p>Loading...</p>
          }
          </div>
        </div>
    )   
  }
}

const mapStateToProps = state => ({ 
  currencyPairs: state.currencyPairs,
  rates: state.rates,
  selected: state.selected
});

const mapDispatchToProps = dispatch => ({
  setCurrencyPairs: pairs => dispatch(setCurrencyPairs(pairs)),
  setRates: rates => dispatch(setRates(rates)),
  selectPair: pair => dispatch(selectPair(pair))
});

export default connect(mapStateToProps, mapDispatchToProps)(App)