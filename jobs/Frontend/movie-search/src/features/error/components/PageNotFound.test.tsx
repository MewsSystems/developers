import { render, screen } from '@testing-library/react';
import PageNotFound from './PageNotFound';

it('renders page-not-found-content', async () => {
  render(<PageNotFound />);

  const notFoundContent = screen.getByTestId('page-not-found-content');
  expect(notFoundContent).toBeInTheDocument();
});
