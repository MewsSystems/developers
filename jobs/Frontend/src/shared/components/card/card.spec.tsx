import React from 'react';
import { render } from '@testing-library/react';
import Card from './card';

describe('Card component', () => {
  it('renders correctly', () => {
    const imageUrl = 'https://example.com/image.jpg';
    const title = 'Test Title';
    const subtitle = 'Test Subtitle';
    const description = 'Test Description';

    const { getByText, getByAltText } = render(
      <Card
        imageUrl={imageUrl}
        title={title}
        subtitle={subtitle}
        description={description}
      />,
    );

    expect(getByText(title)).toBeInTheDocument();
    expect(getByText(subtitle)).toBeInTheDocument();
    expect(getByText(description)).toBeInTheDocument();
    expect(getByAltText('Image')).toBeInTheDocument();
  });
});
