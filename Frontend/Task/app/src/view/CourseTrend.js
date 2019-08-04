// @flow strict

import * as React from 'react';

import Text from '../components/Text';
import { TRENDS } from '../utils/constants';

type Props = {|
  +currentCourse: number,
  +previousCourse?: number,
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
      <Text element="span" color="#4bb543" bold>
        {TRENDS.GROWING}
      </Text>
    );
  }

  return (
    <Text element="span" color="#D8000C" bold>
      {TRENDS.DECLINING}
    </Text>
  );
};

export default Trend;
