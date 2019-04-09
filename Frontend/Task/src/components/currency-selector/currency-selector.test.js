import React from 'react';
import test from 'ava';
import { shallow } from 'enzyme';
import Select from 'react-select';
import { CurrencySelector } from './currency-selector.jsx';

const noop = () => {};

test('CurrencySelector component should be a function', t => {
  t.is(typeof CurrencySelector, 'function');
});

test('CurrencySelector should render select component', t => {
  const wrapper = shallow(
    <CurrencySelector handleChange={noop} />,
  );
  t.truthy(wrapper.find(Select)
    .exists());
});
