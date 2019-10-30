import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';

import CurrencyList from './currency-list';
import { searchText } from '../actions';

class App extends Component {
  onTextChange(text) {
    const { dispatch } = this.props;
    dispatch(searchText(text));
  }

  render() {
    return (
      <div className="main">
        <nav className="navbar navbar-light bd-navbar row">
          <div className="col-md-6 d-flex justify-content-center">
            <h3 className="navbar-brand m-0" href="">
              Currency Converter
            </h3>
          </div>
          <div className="col-md-6 d-flex">
            <form className="d-inline-block flex-grow-1">
              <div className="form-control d-flex">
                <i className="fas fa-search mr-2 mt-1" />
                <input
                  id="input-text"
                  type="text"
                  className="border-0 flex-grow-1"
                  placeholder="Currency Pair Filter"
                  onChange={event => this.onTextChange(event.target.value)}
                />
              </div>
            </form>
            <a className="d-inline-block">
              <i className="fab fa-github fa-2x github-icon mt-1 ml-5 align-middle" />
            </a>
          </div>
        </nav>
        <CurrencyList />
      </div>
    );
  }
}

App.propTypes = {
  dispatch: PropTypes.func.isRequired,
};

function mapStateToProps(state) {
  return {
    text: state.text,
  };
}

export default connect(mapStateToProps)(App);
