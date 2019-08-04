// @flow strict

import * as React from 'react';

import Text from '../components/Text';
import { TRENDS, COLORS } from '../utils/constants';

type Props = {|
  +currentCourse: number,
  +previousCourse?: ?number,
|};

const Trend = ({ currentCourse, previousCourse }: Props) => {
  if (!previousCourse || previousCourse === currentCourse) {
    return (
      <Text element="span" bold>
        {TRENDS.STAGNATING}
      </Text>
    );
  }

  if (Math.max(currentCourse, previousCourse) === currentCourse) {
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
