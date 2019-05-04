import React from 'react';
import PropTypes from 'prop-types';
import './style.css';

class PairRow extends React.Component {
  shouldComponentUpdate(nextProps) {
    const { rate } = this.props;
    return nextProps.rate !== rate;
  }

  getTrend = () => {
    const { rate, oldRate } = this.props;
    let trend = null;

    if (!rate || !oldRate) {
      return null;
    }

    if (rate === oldRate) {
      trend = 'stagnating';
    } else if (rate < oldRate) {
      trend = 'declining';
    } else if (rate > oldRate) {
      trend = 'growing';
    }
    return trend;
  };

  render() {
    const { pair, rate, oldRate } = this.props;
    const trend = this.getTrend();
    return (
      <tr>
        <td>
          {pair}
        </td>
        <td>
          {!rate ? <i>fetching rate</i> : rate}
          {oldRate ? (
            <small title="previous rate" className="text-secondary super-small"><i>{`(${oldRate})`}</i></small>
          ) : null}

        </td>
        <td className="text-right">
          <span className={trend}>
            {!trend ? <i>waiting for data change</i> : trend}
          </span>
        </td>
      </tr>
    );
  }
}

PairRow.propTypes = {
  rate: PropTypes.number,
  oldRate: PropTypes.number,
  pair: PropTypes.string,
};

PairRow.defaultProps = {
  rate: null,
  oldRate: null,
  pair: null,
};

export default PairRow;
