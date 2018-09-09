import React, { Component } from 'react';
import { connect } from 'react-redux';
import { fetchPairs } from '../middleware/api';
import PropTypes from 'prop-types';

class Pairs extends Component {
  componentDidMount() {
    this.props.fetchPairs();
  }

  render() {
    const { currencyPairs } = this.props;
    return (
      <div>
        <h1>
          {' '}
          <div>
            <ul>
              {Object.keys(currencyPairs).map(key => (
                <li key={key}>
                  {currencyPairs[key][0].code} - {currencyPairs[key][1].code}
                  &emsp;
                </li>
              ))}
            </ul>
          </div>
        </h1>
      </div>
    );
  }
}

Pairs.propTypes = {
  currencyPairs: PropTypes.object.isRequired
};

const mapStateToProps = state => ({
  currencyPairs: state.pairs.currencyPairs,
  loading: state.pairs.loading,
  error: state.pairs.error
});

export default connect(
  mapStateToProps,
  { fetchPairs }
)(Pairs);
