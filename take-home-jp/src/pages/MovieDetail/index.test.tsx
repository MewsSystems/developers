import React from 'react';
import renderer from 'react-test-renderer';
import MoveDetail from './';

it('renders correctly', () => {
  const tree = renderer.create(<MoveDetail />).toJSON();
  expect(tree).toMatchSnapshot();
});
