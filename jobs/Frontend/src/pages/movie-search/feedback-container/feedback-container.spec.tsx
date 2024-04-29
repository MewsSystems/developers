import React from 'react';
import { render } from '@testing-library/react';
import FeedbackContainer from './feedback-container';

describe('FeedbackContainer component', () => {
  it('renders title and subtitle correctly', () => {
    const title = 'Test Title';
    const subtitle = 'Test Subtitle';

    const { getByText } = render(
      <FeedbackContainer title={title} subtitle={subtitle} />,
    );

    expect(getByText(title)).toBeInTheDocument();
    expect(getByText(subtitle)).toBeInTheDocument();
  });

  it('renders only title when subtitle is not provided', () => {
    const title = 'Test Title';

    const { getByText, queryByText } = render(
      <FeedbackContainer title={title} />,
    );

    expect(getByText(title)).toBeInTheDocument();
    expect(queryByText('Test Subtitle')).toBeNull();
  });
});
