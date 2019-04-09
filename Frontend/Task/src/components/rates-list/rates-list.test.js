import React from 'react';
import test from 'ava';
import { shallow } from 'enzyme';
import { RatesList } from './rates-list.jsx';
import { RatesListItemComponent } from './index';

test('RatesList component should be a function', t => {
  t.is(typeof RatesList, 'function');
});

test('RatesList should render ul element', t => {
  const wrapper = shallow(
    <RatesList items={[]} />,
  );
  t.is(wrapper.find('ul').exists(), true);
});

test('RatesList should render RatesListItems for every provided item', t => {
  const wrapper = shallow(
    <RatesList items={[
      {
        value: 1,
        label: 'A',
        next: 1,
        prev: 1,
      },
      {
        value: 2,
        label: 'B',
        next: 2,
        prev: 2,
      },
    ]}
    />,
  );

  t.is(wrapper.find(RatesListItemComponent).exists(), true);
});
