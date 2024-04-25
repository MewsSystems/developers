import { setUp } from '../../utils/testUtils';
import { SearchControls } from './SearchControls';
import { screen } from '@testing-library/react';

const mockIncrementPageNumber = jest.fn().mockName('mockIncrementPageNumber');
const mockDecrementPageNumber = jest.fn().mockName('mockDecrementPageNumber');

describe('SearchControls', () => {
  it('renders no controls when showControls is false', async () => {
    const { container } = setUp(
      <SearchControls
        showControls={false}
        page={1}
        numberOfPages={1}
        incrementPageNumber={mockIncrementPageNumber}
        decrementPageNumber={mockDecrementPageNumber}
        id={'top'}
      />,
    );

    expect(container).toBeEmptyDOMElement();
  });

  it('renders the controls with a number of pages', async () => {
    const { rerender } = setUp(
      <SearchControls
        showControls={true}
        page={1}
        numberOfPages={10}
        incrementPageNumber={mockIncrementPageNumber}
        decrementPageNumber={mockDecrementPageNumber}
        id={'top'}
      />,
    );

    expect(await screen.findByText(/1 of 10/i)).toBeInTheDocument();

    rerender(
      <SearchControls
        showControls={true}
        page={8}
        numberOfPages={25}
        incrementPageNumber={mockIncrementPageNumber}
        decrementPageNumber={mockDecrementPageNumber}
        id={'top'}
      />,
    );

    expect(await screen.findByText(/8 of 25/i)).toBeInTheDocument();
  });
});
