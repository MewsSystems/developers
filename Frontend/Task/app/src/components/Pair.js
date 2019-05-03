import React from 'react';
import PropTypes from 'prop-types';

class Pair extends React.Component {
  state = { trend: null };

  shouldComponentUpdate(nextProps) {
    const { rate } = this.props;
    return nextProps.rate !== rate;
  }

  componentDidUpdate(prevProps) {
    const { rate } = this.props;
    this.setTrend(prevProps.rate, rate);
  }

  setTrend = (prev, next) => {
    let trend = 'growing';
    if (prev === next) {
      trend = 'stagnating';
    } else if (prev < next) {
      trend = 'growing';
    }
    this.setState({ trend });
  };

  render() {
    const { pairs, rate } = this.props;
    const [pairOne, pairTwo] = pairs;
    const { trend } = this.state;

    return (
      <tr>
        <td>
          {`${pairOne.code}/${pairTwo.code}`}
        </td>
        <td>
          {!rate ? <i>fetching course</i> : rate}
        </td>
        <td>
          {!trend ? <i>waiting for data</i> : trend}
        </td>
      </tr>
    );
  }
}

Pair.propTypes = {
  rate: PropTypes.number,
  pairs: PropTypes.arrayOf(PropTypes.objectOf(PropTypes.string)).isRequired,
};

Pair.defaultProps = {
  rate: null,
};

export default Pair;
