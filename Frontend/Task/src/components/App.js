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
        <nav className="navbar navbar-light">
          <h5 className="navbar-brand mx-auto mb-2" href="">
            Currency Converter
          </h5>
          <form className="form-inline my-2 my-lg-0">
            <div className="form-control mr-sm-2">
              <i className="fas fa-search" />
              <input
                type="text"
                className="ml-2 border-0"
                placeholder="Currency Pair Filter"
                onChange={event => this.onTextChange(event.target.value)}
              />
            </div>
          </form>
          <a href="xxx-Github" target="_blank">
            <i className="fab fa-github fa-2x" />
          </a>
          <CurrencyList />
        </nav>
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
