import React, { Component } from 'react';
import { connect } from 'react-redux';
import { PropTypes } from 'prop-types';
import { Select, MenuItem, InputLabel, FormControl } from '@material-ui/core';

import Cookies from 'universal-cookie';

import styles from './style.scss';

const cookie = new Cookies();

/**
 * Pairs select component
 */
class PairsSelect extends Component {
  static propTypes = {
    currencyPairs: PropTypes.object,
    updateState: PropTypes.func.isRequired,
    value: PropTypes.array,
  };

  state = {
    pairs: [],
  };

  componentDidUpdate(prevProps, prevState) {
    if (this.props.currencyPairs !== prevProps.currencyPairs) {
      this.setPairs();
    }
  }

  setPairs() {
    const { currencyPairs } = this.props;

    let pairs = [];

    /**
     * Collect data
     * for select component
     **/
    Object.keys(currencyPairs).forEach((key) => {
      pairs.push({
        key,
        value: currencyPairs[key],
        name: `${currencyPairs[key][0].name} / ${currencyPairs[key][1].name}`,
      })
    });

    this.filterPairsByCookieValues(pairs);

    this.props.updateState({
      name: 'selectValue',
      value: [...this.filterPairsByCookieValues(pairs).map((pair) => pair.key)] });

    this.setState({ pairs });
  }

  /**
   * Filter currency pair by cookie values if exist
   * @param {Array} pairs
   * @return {*}
   */
  filterPairsByCookieValues(pairs) {
    /** Load data from cookie and filter pairs **/
    const cookieConfig = cookie.get('config');

    if (cookieConfig) {
      return pairs.filter((pair) => cookieConfig.indexOf(pair.key) !== -1);
    }

    return pairs;
  }

  render() {
    const { updateState, value } = this.props;
    const { pairs } = this.state;

    return (
      <div className={styles.wrap}>
        <FormControl
          classes={{ root: styles.control }}
        >
          <InputLabel
            classes={{ root: styles.label }}
          >Currencies</InputLabel>
          <Select
            variant="outlined"
            labelId="Pairs"
            value={value || []}
            multiple
            onChange={(e) => {
              updateState({ name: 'selectValue', value: e.target.value });
              cookie.set('config', e.target.value);
            }}
          >
            {pairs.map((pair, index) => <MenuItem key={index} value={pair.key}>{pair.name}</MenuItem>)}
          </Select>
        </FormControl>
      </div>
    );
  }
}

const mapStateToProps = (state) => ({
  currencyPairs: state.currency.currencyPairs,
});

export default connect(mapStateToProps)(PairsSelect);