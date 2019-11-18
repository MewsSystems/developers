import React, { Component } from 'react';
import { connect } from 'react-redux';
import { PropTypes } from 'prop-types';

import { Select, MenuItem } from '@material-ui/core';

class PairsSelect extends Component {
  static propTypes = {
    currencyPairs: PropTypes.object,
  };

  state = {
    pairs: [],
  };

  componentDidUpdate(prevProps, prevState) {
    if (this.props.currencyPairs !== prevProps.currencyPairs) {
      const { currencyPairs } = this.props;
      const pairs = [];
      Object.keys(currencyPairs).forEach((key) => {
        pairs.push({
          value: currencyPairs[key],
          key,
          name: `${currencyPairs[key][0].name} / ${currencyPairs[key][1].name}`,
        })
      });

      this.setState({ pairs });
    }
  }

  render() {
    const { pairs, selectValue } = this.state;

    return (
      <div>
        <Select
          labelId="Pairs"
          value={selectValue || ''}
          onChange={(e) => this.setState({ selectValue: e.target.value })}
        >
          {pairs.map((pair) => <MenuItem value={pair.key}>{pair.name}</MenuItem>)}
        </Select>
      </div>
    );
  }
}

const mapStateToProps = (state) => ({
  currencyPairs: state.currency.currencyPairs,
});

export default connect(mapStateToProps)(PairsSelect);