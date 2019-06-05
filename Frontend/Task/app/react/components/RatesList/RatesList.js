import React, { Component } from 'react';
import { connect } from 'react-redux';
import {TIMEOUT} from '../../constants/timeout.js';

class RatesList extends Component {
  
  constructor() {
    super();
    this.countDown = ::this.countDown;
    
    this.state = {
      timeBeforeUpdate: TIMEOUT / 1000,
    }
  }
  
  componentDidUpdate(prevProps, prevState, snapshot) {
    const { timeBeforeUpdate } = prevState;
    const oldRates = prevProps.rates;
    const { rates } = this.props;
    
    if (oldRates.length !== rates.length) {
      clearInterval(this.countDownIntrevalId);
      
      this.setState({
        timeBeforeUpdate: 10
      });
      
      this.countDownIntrevalId = setInterval(this.countDown, 1000);
    }
    
    
    if (timeBeforeUpdate >= 9 && rates.length > 0) {
      
      clearInterval(this.countDownIntrevalId);
      this.countDownIntrevalId = setInterval(this.countDown, 1000);
      
    } else if (rates.length < 1) {
      
      clearInterval(this.countDownIntrevalId);
      
    }
  }
  
  countDown() {
    if (this.state.timeBeforeUpdate > 1) {
      const seconds = this.state.timeBeforeUpdate - 1;
      this.setState({
        timeBeforeUpdate: seconds
      })
    } else {
      this.setState({
        timeBeforeUpdate: TIMEOUT / 1000
      });
    }
  }
  
  render() {
    const { rates } = this.props;
    return (
      <div className='rates'>
        <h2>Rates list</h2>
        <ul className='rates--list'>
          {
            rates.map(rate => {
              return (
                <li className='rates--list--item' key={rate.id}>
                  <span className='rate-label'>{rate.label}</span>
                  <span className='rate-rate'>{rate.rate}</span>
                  <small className={'rate-dynamic rate-dynamic__' + rate.dynamic}>{rate.dynamic}</small>
                </li>
              );
            })
          }
        </ul>
        {
          rates.length > 0 &&
          <small> (next update in: {this.state.timeBeforeUpdate} sec.)</small>
        }
      </div>
    );
  }
}

const mapStateToProps = state => ({
  rates: state.currencies.rates,
});

export default connect(mapStateToProps, null)(RatesList);
