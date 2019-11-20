import React, { Component } from 'react';
import { connect } from 'react-redux';
import { PropTypes } from 'prop-types';
import { Select, MenuItem, InputLabel, FormControl } from '@material-ui/core';

import styles from './style.scss';

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
      const { currencyPairs } = this.props;

      const pairs = [];

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

      this.props.updateState({ name: 'selectValue', value: [...pairs.map((pair) => pair.key)] });

      this.setState({ pairs });
    }
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
            onChange={(e) => updateState({ name: 'selectValue', value: e.target.value })}
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