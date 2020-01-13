import React, { Component, } from 'react';
import {
  func, arrayOf, shape, string, object,
} from 'prop-types';
import TrendIcon from '../../../../components/TrendIcon/TrendIcon';


class RowsView extends Component {
  componentDidMount() {
    const { startRatesInterval, } = this.props;

    startRatesInterval();
  }


  render() {
    const {
      rows,
      rates,
    } = this.props;

    return (
      <tbody>
        {rows.map((row) => {
          const { id, name, } = row;
          const rate = Object.prototype.hasOwnProperty.call(rates, id)
            ? rates[id].rate
            : null;
          const trend = Object.prototype.hasOwnProperty.call(rates, id)
            ? rates[id].trend
            : null;

          return (
            <tr key={id}>
              <td>
                {name}
              </td>
              <td>
                {rate === null ? '-' : rate}
              </td>
              <td className="rateList--table-td3">
                <TrendIcon trend={trend} />
              </td>
            </tr>
          );
        })}
      </tbody>
    );
  }
}


RowsView.propTypes = {
  rows: arrayOf(shape({
    name: string.isRequired,
  })).isRequired,
  rates: object.isRequired,
  startRatesInterval: func.isRequired,
};


export default RowsView;
