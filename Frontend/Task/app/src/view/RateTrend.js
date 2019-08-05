// @flow strict

import * as React from 'react';

import Text from '../components/Text';
import { TRENDS, COLORS } from '../utils/constants';

type Props = {|
  +currentRate: number,
  +previousRate?: ?number,
|};

const Trend = ({ currentRate, previousRate }: Props) => {
  if (!previousRate || previousRate === currentRate) {
    return (
      <Text element="span" bold>
        {TRENDS.STAGNATING}
      </Text>
    );
  }

  if (Math.max(currentRate, previousRate) === currentRate) {
    return (
      <Text element="span" color={COLORS.SUCCESS} bold>
        {TRENDS.GROWING}
      </Text>
    );
  }

  return (
    <Text element="span" color={COLORS.CRITICAL} bold>
      {TRENDS.DECLINING}
    </Text>
  );
};

export default Trend;
