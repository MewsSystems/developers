import { render, screen } from '@testing-library/react';
import Home from './page';

test('renders heading', () => {
  render(<Home />);
  expect(screen.getByRole('heading', { name: /home/i })).toBeInTheDocument();
});
