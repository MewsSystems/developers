import { render, screen } from '@testing-library/react';
import { Header } from './Header';

describe('Header', () => {
  it('renders', async () => {
    render(<Header />);

    expect(
      await screen.findByRole('heading', { name: /search for movies/i }),
    ).toBeInTheDocument();
  });
});
