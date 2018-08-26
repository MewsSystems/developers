// @flow
import React, { PureComponent } from 'react';

import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import * as actionCreators from 'Redux/actions';

import isEqual from 'lodash.isequal';
import trend from 'trend';

import ExchangeCard from 'Components/exchangeCard';

type Props = {
  pairInfo: Object,
  currencyTrends: Object,
  updateTrendData: Function,
};
type State = {
  trendDirection: ?string,
};

class ExchangeCardContainer extends PureComponent<Props, State> {
  state = {
    trendDirection: 'flat',
  };

  componentWillReceiveProps(nextProps: Object) {
    const {
      pairInfo: currentPairInfo,
      updateTrendData,
      currencyTrends,
    } = this.props;
    const { pairInfo: nextPairInfo } = nextProps;
    const { pairKey, rate } = nextPairInfo;

    if (!isEqual(nextPairInfo, currentPairInfo)) {
      updateTrendData(pairKey, rate);
    }

    if (currencyTrends[pairKey] && currencyTrends[pairKey].length > 2) {
      const trends = currencyTrends[pairKey];
      const points = () => {
        if (trends.length < 20) {
          return trends.length;
        }
        return 20;
      };
      const growth = trend(trends, {
        lastPoints: 1,
        avgPoints: points,
        reversed: false,
      });

      if (growth < 0.98) {
        this.setState({ trendDirection: 'down' });
      } else if (growth > 1.02) {
        this.setState({ trendDirection: 'up' });
      } else {
        this.setState({ trendDirection: 'flat' });
      }
    }
  }

  render() {
    const { pairInfo } = this.props;
    const { trendDirection } = this.state;

    return (
      <ExchangeCard
        key={pairInfo.pairKey}
        pairInfo={pairInfo}
        trendDirection={trendDirection}
      />
    );
  }
}
function mapStateToProps(state) {
  return { currencyTrends: state.trends };
}

function mapDispatchToProps(dispatch) {
  return bindActionCreators(actionCreators, dispatch);
}

export default connect(
  mapStateToProps,
  mapDispatchToProps,
)(ExchangeCardContainer);
