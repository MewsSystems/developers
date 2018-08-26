import React from 'react';
import { shallow } from 'enzyme';
import renderer from 'react-test-renderer';
import ExchangeCard from '../index';

const pairInfo = {
  first: {
    name: 'ABC',
    code: 'XYZ',
  },
  second: {
    name: 'DEF',
    code: 'UVW',
  },
  rate: 1.12345,
  pairKey: '70c6744c-cba2-5f4c-8a06-0dac0c4e43a1',
};

const trendDirection = 'down';

describe('Exchange Card', () => {
  const component = shallow(
    <ExchangeCard
      key={pairInfo.pairKey}
      pairInfo={pairInfo}
      trendDirection={trendDirection}
    />,
  );

  it('renders 1 component', () => {
    expect(component).toHaveLength(1);
  });

  it('matches snapshot', () => {
    const layout = renderer
      .create(
        <ExchangeCard
          key={pairInfo.pairKey}
          pairInfo={pairInfo}
          trendDirection={trendDirection}
        />,
      )
      .toJSON();
    expect(layout).toMatchSnapshot();
  });
});
