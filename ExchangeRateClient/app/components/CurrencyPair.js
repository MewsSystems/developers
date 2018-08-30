import React, { Component } from 'react';
import { endpoint, interval } from '../config';
// redux
import { connect } from 'react-redux';
import { setRates } from '../store/actions';


class CurrencyPair extends Component {
  constructor(props) {
    super(props);

    this.state = {
      trend: "--",
    }

    this.getTrend = this.getTrend.bind(this);
    this.getRates = this.getRates.bind(this);
  }

  componentDidMount() {
    this.getRates(this.props.pair)
  }
  

  getRates(pair) {
    if(pair !== undefined) {
      fetch(endpoint + `/rates?currencyPairIds[]=${pair}`)
      .then(response => {
        if(!response.status == 200) return;
        return response.json()
      })
      .then(data => {
        const { currencyPairs } = this.props;

        if(data && data.rates !== undefined) {
          const prev = (currencyPairs[pair][2] !== undefined) && currencyPairs[pair][2].rate;  
          const next = data.rates[pair];
          
          this.props.setRates(next, pair);
          this.getTrend(prev, next);
        }
      })

      setTimeout(() => this.getRates(pair), interval)
    }
  }

  getTrend(prev, next) {
    if(prev < next) {
      this.setState({ trend: "growing" });
    }
    if(prev > next) {
      this.setState({ trend: "declining" });
    }
    if(prev == next) {
      this.setState({ trend: "stagnating" });
    }
  }

  render() {
    const { currencyPairs, pair, rates } = this.props;
    const { trend } = this.state;

    const card = {
      padding: '10px',
      border: '1px solid #ddd',
      flexBasis: '280px',
      marginRight: '10px',
      marginBottom: '10px',
      fontFamily: 'Verdana',
      fontSize: '10px'
    }
    const rateTrend = {
      stagnating: { color: 'orange' },
      growing: { color: '#36c736' },
      declining: { color: 'red' }
    }

    return (
      <div style={card}>
        {`${currencyPairs[pair][0].name} / ${currencyPairs[pair][1].name}`}
        <div>
          {
            (currencyPairs[pair][2] !== undefined && currencyPairs[pair][2].rate !== undefined)
            && currencyPairs[pair][2].rate
          }
        </div>
        <div style={rateTrend[trend]}>
          {(trend !== undefined) ? trend : "stagnating"}
        </div>
      </div>
    )
  }
}

const mapStateToProps = state => ({ 
  currencyPairs: state.currencyPairs,
});

const mapDispatchToProps = dispatch => ({
  setRates: (rate, pair) => dispatch(setRates(rate, pair))
});

export default connect(mapStateToProps, mapDispatchToProps)(CurrencyPair)