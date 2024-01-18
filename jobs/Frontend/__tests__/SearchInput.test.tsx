import { renderWithProviders } from '@/utils/test-utils';
import { screen } from '@testing-library/react'
import SearchInput from '@/components/SearchInput';

test('Correctly Renders the Search', () => {
  renderWithProviders(<SearchInput/>);
  expect(screen.getByText(/Search Movie:/i));
});
