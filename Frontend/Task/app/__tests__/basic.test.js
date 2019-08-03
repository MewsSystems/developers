// @flow strict

import * as React from 'react';
import { render } from '@testing-library/react';

const TestComponent = () => <span>Test</span>;

describe('TestComoponent', () => {
  it('renders properly', () => {
    const { getByText } = render(<TestComponent />);

    expect(getByText('Test')).toBeInTheDocument();
  });
});
