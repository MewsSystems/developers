import React from 'react';

class Pair extends React.Component {

  state = {trend: 'declining'};

  shouldComponentUpdate(nextProps) {
    return nextProps.rate !== this.props.rate;
  }

  componentDidUpdate(prevProps) {
    this.setTrend(prevProps.rate, this.props.rate);
  }

  setTrend = (prev, next) => {
    let trend = 'growing';
    if (prev === next) {
      trend = 'declining';
    } else if (prev < next) {
      trend = 'growing';
    }
    this.setState({trend})
  };

  getTrend = () => {
    return this.state.trend;
  };

  render() {
    const [pairOne, pairTwo] = this.props.pairs;
    return (
      <tr>
        <td>
          {`${pairOne.code}/${pairTwo.code}`}
        </td>
        <td>
          {this.props.rate}
        </td>
        <td>
          {this.getTrend()}
        </td>
      </tr>
    );
  }
}

export default Pair;
