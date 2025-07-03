import { render, screen, fireEvent } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { Pagination, PaginationProps } from './Pagination';

describe('Pagination', () => {
  const baseProps: Omit<PaginationProps, 'onPageChange'> = {
    currentPage: 3,
    totalPages: 5,
    search: 'test',
  };

  it('renders navigation landmark with accessible name', () => {
    render(<Pagination {...baseProps} onPageChange={vi.fn()} />);

    expect(screen.getByRole('navigation', { name: /pagination/i })).toBeInTheDocument();
  });

  it('renders both first and last page links on a middle page', () => {
    render(<Pagination {...baseProps} onPageChange={vi.fn()} />);

    expect(screen.getByRole('link', { name: /go to first page/i })).toBeInTheDocument();
    expect(screen.getByRole('link', { name: /go to last page/i })).toBeInTheDocument();
  });

  it('does not render "go to first page" link on the first page', () => {
    render(<Pagination {...baseProps} currentPage={1} onPageChange={vi.fn()} />);

    expect(screen.queryByRole('link', { name: /go to first page/i })).not.toBeInTheDocument();
  });

  it('does not render "go to last page" link on the last page', () => {
    render(<Pagination {...baseProps} currentPage={5} onPageChange={vi.fn()} />);

    expect(screen.queryByRole('link', { name: /go to last page/i })).not.toBeInTheDocument();
  });

  it('renders both prev and next when on a middle page', () => {
    render(<Pagination {...baseProps} onPageChange={vi.fn()} />);

    expect(screen.getByRole('link', { name: /previous page/i })).toBeInTheDocument();
    expect(screen.getByRole('link', { name: /next page/i })).toBeInTheDocument();
  });

  it('hides prev when on the first page', () => {
    render(<Pagination {...baseProps} currentPage={1} onPageChange={vi.fn()} />);

    expect(screen.queryByRole('link', { name: /previous page/i })).not.toBeInTheDocument();
    expect(screen.getByRole('link', { name: /next page/i })).toBeInTheDocument();
  });

  it('hides next when on the last page', () => {
    render(<Pagination {...baseProps} currentPage={5} onPageChange={vi.fn()} />);

    expect(screen.getByRole('link', { name: /previous page/i })).toBeInTheDocument();
    expect(screen.queryByRole('link', { name: /next page/i })).not.toBeInTheDocument();
  });

  it('calls onPageChange with the correct page when a page number is clicked', async () => {
    const onPageChange = vi.fn();
    render(<Pagination {...baseProps} onPageChange={onPageChange} />);

    await userEvent.click(screen.getByRole('link', { name: 'Go to page 4' }));

    expect(onPageChange).toHaveBeenCalledWith(4);
  });

  it('calls onPageChange with correct page when navigation arrows are clicked', async () => {
    const onPageChange = vi.fn();
    render(<Pagination {...baseProps} onPageChange={onPageChange} />);

    await userEvent.click(screen.getByRole('link', { name: /previous page/i }));

    expect(onPageChange).toHaveBeenCalledWith(2);

    const next = screen.getByRole('link', { name: /next page/i });

    await userEvent.click(next);

    expect(onPageChange).toHaveBeenCalledWith(4);
  });

  it('calls onPageChange with correct page when first/last links are clicked', async () => {
    const onPageChange = vi.fn();
    render(<Pagination {...baseProps} onPageChange={onPageChange} />);

    await userEvent.click(screen.getByRole('link', { name: /go to first page/i }));

    expect(onPageChange).toHaveBeenCalledWith(1);

    await userEvent.click(screen.getByRole('link', { name: /go to last page/i }));

    expect(onPageChange).toHaveBeenCalledWith(5);
  });

  it('shows the correct aria-current on the current page', () => {
    render(<Pagination {...baseProps} onPageChange={vi.fn()} />);

    const current = screen.getByRole('link', { current: 'page' });

    expect(current).toHaveAttribute('aria-current', 'page');

    expect(current).toHaveTextContent(baseProps.currentPage.toString());
  });

  it('renders readonly state correctly', async () => {
    const onPageChange = vi.fn();
    render(<Pagination {...baseProps} readonly onPageChange={onPageChange} />);
    const page4 = screen.getByRole('link', { name: 'Go to page 4' });

    await userEvent.click(page4);

    expect(onPageChange).not.toHaveBeenCalled();

    expect(page4).toHaveAttribute('aria-disabled', 'true');

    screen.getAllByRole('link').forEach((el) => expect(el).toHaveAttribute('tabindex', '-1'));
  });

  it('renders disableKeyboardNav state correctly', () => {
    render(<Pagination {...baseProps} disableKeyboardNav onPageChange={vi.fn()} />);

    screen.getAllByRole('link').forEach((el) => expect(el).toHaveAttribute('tabindex', '-1'));
  });

  it('does not call onPageChange if meta, ctrl, alt, shift, or non-left mouse is used', () => {
    const handler = (event: MouseEvent) => {
      const target = event.target as HTMLElement;
      if (target.tagName === 'A') {
        event.preventDefault();
      }
    };

    // workaround for jsdom error message - https://tinyurl.com/mrybwreb
    document.addEventListener('click', handler, true);

    const onPageChange = vi.fn();
    render(<Pagination {...baseProps} onPageChange={onPageChange} />);
    const page4 = screen.getByRole('link', { name: 'Go to page 4' });

    fireEvent.click(page4, { metaKey: true });
    fireEvent.click(page4, { altKey: true });
    fireEvent.click(page4, { ctrlKey: true });
    fireEvent.click(page4, { shiftKey: true });
    fireEvent.click(page4, { button: 2 });

    expect(onPageChange).not.toHaveBeenCalled();

    document.removeEventListener('click', handler, true);
  });

  it('applies additional HTMLAttributes passed in', () => {
    render(
      <Pagination
        {...baseProps}
        onPageChange={vi.fn()}
        data-testid="custom-pagination"
        aria-label="Custom Label"
      />
    );

    expect(screen.getByTestId('custom-pagination')).toBeInTheDocument();

    expect(screen.getByRole('navigation', { name: 'Custom Label' })).toBeInTheDocument();
  });
});
