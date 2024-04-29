import React from 'react';
import { render } from '@testing-library/react';
import Feedback from './feedback';

describe('Feedback component', () => {
  it('renders with correct title and subtitle', () => {
    const title = 'Thank you for your feedback!';
    const subtitle = 'We appreciate your valuable input.';

    const { getByText } = render(
      <Feedback title={title} subtitle={subtitle} />,
    );

    expect(getByText(title)).toBeInTheDocument();
    expect(getByText(subtitle)).toBeInTheDocument();
  });
});
