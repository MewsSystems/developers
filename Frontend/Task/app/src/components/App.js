import React, {Component} from 'react';
import apiCall from '../api/apiCall';
import {connect} from 'react-redux';

import PairSelector from './PairSelector';
import RatesTable from './RatesTable';

import '../styles.css';

import {setCurrencyPairs} from '../actions/pairsActions';

class App extends Component {
  constructor () {
    super ();
    this.state = {
      fetching: true,
    };
  }
  fetchConfiguration = () => {
    apiCall ('/configuration', (err, res) => {
      if (err) return this.fetchConfiguration ();

      this.props.setCurrencyPairs (res.currencyPairs);
      this.setState ({fetching: false});
    });
  };

  componentDidMount () {
    this.fetchConfiguration ();
  }

  render () {
    const loading = this.state.fetching;

    if (loading)
      return (
        <div className="loader-container">
          <div className="loader" />
        </div>
      );

    return (
      <div style={{margin: '0 auto', maxWidth: '90%', marginBottom: 100}}>
        <PairSelector />
        <RatesTable />
      </div>
    );
  }
}

const mapStateToProps = state => {
  return {
    currencyPairs: state.pairsReducer.currencyPairs,
  };
};

const mapDispatchToProps = dispatch => {
  return {
    setCurrencyPairs: currencyPairs => {
      dispatch (setCurrencyPairs (currencyPairs));
    },
  };
};

export default connect (mapStateToProps, mapDispatchToProps) (App);
