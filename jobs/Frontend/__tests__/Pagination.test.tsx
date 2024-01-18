import { act, fireEvent, render, screen } from '@testing-library/react';
import Pagination, { PaginationProps } from '@/components/Pagination/Pagination';
import exp from 'constants';

const defaultProps: PaginationProps = {
  currentPage: 1,
  onNextPage: jest.fn(),
  onPrevPage: jest.fn(),
  onSetPage: jest.fn(),
  totalPages: 1
}

test('Correctly Renders the Pagination', () => {
  render(<Pagination {...defaultProps}/>);
  expect(screen.getByText(/First/i));
  expect(screen.getByText(/Last/i));
  expect(screen.getByText(/Next/i));
  expect(screen.getByText(/Prev/i));
  expect(screen.getByText(/1/i));
});

test('Correctly Renders number of pagination buttons', () => {
  render(<Pagination {...defaultProps} totalPages={5}/>);
  expect(screen.getByText(/5/i));
});

test('Call setPage on direct Click', async () => {
  render(<Pagination {...defaultProps} totalPages={5}/>);
  const button = screen.getByTestId('page-3-button');
  fireEvent.click(button);
  expect(defaultProps.onSetPage).toHaveBeenCalledTimes(1);
});
