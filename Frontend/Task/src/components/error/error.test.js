import React from 'react';
import test from 'ava';
import { shallow } from 'enzyme';
import Error from './index';

test('Error component should be a function', t => {
  t.is(typeof Error, 'function');
});

test('Error should render a div with 2 children', t => {
  const wrapper = shallow(
    <Error />,
  );
  t.is(wrapper.find('div').children().length, 2);
});

test('Error should render an i element with fa-exclamation-triangle class', t => {
  const wrapper = shallow(
    <Error />,
  );
  t.is(wrapper.find('i').hasClass('fa-exclamation-triangle'), true);
});
