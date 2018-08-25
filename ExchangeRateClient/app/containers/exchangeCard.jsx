// @flow
import React, { PureComponent } from 'react';
import isEqual from 'lodash.isequal';

import ExchangeCard from 'Components/exchangeCard';

type Props = {
  pairInfo: Object,
};
type State = {
  trend: ?string,
};

class ExchangeCardContainer extends PureComponent<Props, State> {
  state = {
    trend: null,
  };

  componentWillReceiveProps(nextProps: Object) {
    const { pairInfo: currentPairInfo } = this.props;
    const { pairInfo: nextPairInfo } = nextProps;

    if (!isEqual(nextPairInfo, currentPairInfo)) {
      const { rate: currentRate } = currentPairInfo;
      const { rate: nextRate } = nextPairInfo;

      if (nextRate > currentRate) {
        this.setState({ trend: 'up' });
      } else if (nextRate < currentRate) {
        this.setState({ trend: 'down' });
      } else if (nextRate === currentRate) {
        this.setState({ trend: 'flat' });
      }
    }
  }

  render() {
    const { pairInfo } = this.props;
    const { trend } = this.state;
    return (
      <ExchangeCard key={pairInfo.pairKey} pairInfo={pairInfo} trend={trend} />
    );
  }
}

export default ExchangeCardContainer;
