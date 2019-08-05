// @flow

import * as React from 'react';
import { render } from '@testing-library/react';

import { TRENDS } from '../../utils/constants';
import Trend from '../RateTrend';

describe('CourseTrend', () => {
  it('displays stagnating if no previous course is provided', () => {
    const { getByText } = render(<Trend currentRate={2.5} />);

    expect(getByText(TRENDS.STAGNATING)).toBeInTheDocument();
  });

  it('displays stagnating if previous and current course are the same', () => {
    const { getByText } = render(<Trend currentRate={2.5} previousRate={2.5} />);

    expect(getByText(TRENDS.STAGNATING)).toBeInTheDocument();
  });

  it('displays growing if current course is higher than previous', () => {
    const { getByText } = render(<Trend currentRate={2.5} previousRate={1.5} />);

    expect(getByText(TRENDS.GROWING)).toBeInTheDocument();
  });

  it('displays declining if current course is lower than previous', () => {
    const { getByText } = render(<Trend currentRate={1.5} previousRate={2.5} />);

    expect(getByText(TRENDS.DECLINING)).toBeInTheDocument();
  });
});
