import { setUp } from '../../utils/testUtils';
import { SearchBox } from './SearchBox';
import { screen } from '@testing-library/react';

const mockSetSearchQuery = jest.fn().mockName('mockSetSearchQuery');

describe('SearchBox', () => {
  it('calls the onChange handler when a user types in the input', async () => {
    const { user } = setUp(
      <SearchBox searchQuery={'dave'} setSearchQuery={mockSetSearchQuery} />,
    );

    const searchBox = await screen.findByPlaceholderText(
      /enter a movie name to search/i,
    );

    expect(searchBox).toHaveValue('dave');

    await user.type(searchBox, 'steve');

    expect(mockSetSearchQuery).toHaveBeenCalled();
  });
});
