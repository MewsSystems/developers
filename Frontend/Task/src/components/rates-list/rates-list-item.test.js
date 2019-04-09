import React from 'react';
import test from 'ava';
import { shallow } from 'enzyme';
import { RatesListItem } from './rates-list-item.jsx';

test('RatesListItem component should be a function', t => {
  t.is(typeof RatesListItem, 'function');
});

test('RatesListItem should render li element', t => {
  const wrapper = shallow(
    <RatesListItem label='A' next={1} prev={2} />,
  );

  t.is(wrapper.find('li').exists(), true);
});

test('RatesListItem should render 3 span elements', t => {
  const wrapper = shallow(
    <RatesListItem label='A' next={1} prev={2} />,
  );

  t.is(wrapper.find('span').length, 3);
});

test('RatesListItem should render growing icon if next value is greater than previous', t => {
  const wrapper = shallow(
    <RatesListItem label='A' next={3} prev={2} />,
  );

  t.is(wrapper.find('.fa-arrow-alt-circle-up').exists(), true);
});

test('RatesListItem should render declining icon if next value is less than previous', t => {
  const wrapper = shallow(
    <RatesListItem label='A' next={1} prev={2} />,
  );

  t.is(wrapper.find('.fa-arrow-alt-circle-down').exists(), true);
});
