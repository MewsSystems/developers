import { render, screen } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import { expect, it } from 'vitest';
import EnhancedTable from './EnhancedTable';

const MockEnhancedTable = () => (
  <BrowserRouter>
    <EnhancedTable rows={[]} />
  </BrowserRouter>
);

it('renders the enhanced table', () => {
  render(<MockEnhancedTable />);

  const detailsPage = screen.getByTestId('movie-enhanced-table');
  expect(detailsPage).toBeInTheDocument();
});
