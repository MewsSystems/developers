import React, { Component } from 'react';
import { PropTypes } from 'prop-types';
import { connect } from 'react-redux';

import { TableCell, Table, TableHead, TableBody, TableRow, Icon } from '@material-ui/core';
import { CallMade, Remove } from '@material-ui/icons';

import styles from './style.scss';

/**
 * Rate list component
 */
class RateList extends Component {
  static propTypes = {
    rates: PropTypes.object,
    currencyPairs: PropTypes.object,
    currencyKeys: PropTypes.array,
    update: PropTypes.bool,
    setUpdate: PropTypes.func,
  };

  state = {
    prevRates: {},
    filteredCurrencyPairs: [],
  };

  componentDidUpdate(prevProps) {
    if (this.props.update && prevProps.update !== this.props.update) {
      this.props.setUpdate();
      this.setState({ prevRates: prevProps.rates }, () => this.filterData());
    }
  }

  /**
   * Filter data according values from PairSelect component
   * And collect them to present in data grid
   */
  filterData() {
    const { prevRates } = this.state;
    const { currencyPairs, currencyKeys, rates } = this.props;

    const filteredCurrencyPairs = [];

    currencyKeys.forEach((key) => {
      const name = currencyPairs[key].map((pair) => pair.name).join(' / ');
      const currentValue = rates[key];
      const prevValue = prevRates[key];

      const trend = prevValue < currentValue ? 1
        : prevValue > currentValue ? 1 :0;


      filteredCurrencyPairs.push({
        name,
        currentValue,
        trend: trend,
      });
    });

    this.setState({ filteredCurrencyPairs });
  }

  /**
   * Render trend icon
   * @param {number} trend
   * @return {*}
   */
  getTrendIcon(trend) {
    if (trend !== 0) {
      return (
        <CallMade classes={{ root: trend > 0 ? styles.growing : styles.declining }}>
          call_made
        </CallMade>
      )
    }

    return (
      <Remove classes={{ root: styles.stagnating }}>
        remove
      </Remove>
    )
  }

  render() {
    const { filteredCurrencyPairs } = this.state;

    return (
      <div className={styles.wrap}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>
                RATES PAIR
              </TableCell>
              <TableCell>
                CURRENT VALUE
              </TableCell>
              <TableCell>
                TREND
              </TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {filteredCurrencyPairs.map((pair, index) => (
              <TableRow key={index}>
                <TableCell>{pair.name}</TableCell>
                <TableCell>{pair.currentValue}</TableCell>
                <TableCell>{this.getTrendIcon(pair.trend)}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
    );
  }
}

const mapStateToProps = (state) => ({
  currencyPairs: state.currency.currencyPairs,
});

export default connect(mapStateToProps)(RateList);