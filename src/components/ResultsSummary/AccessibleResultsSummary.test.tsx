import { createRef } from 'react';
import { act, render, screen } from '@testing-library/react';
import { AccessibleResultsSummary } from './AccessibleResultsSummary';

describe('AccessibleResultsSummary', () => {
  beforeEach(() => {
    vi.useFakeTimers();
  });

  afterEach(() => {
    vi.runOnlyPendingTimers();
    vi.useRealTimers();
  });

  it('renders no results message when totalItems is 0', () => {
    render(
      <AccessibleResultsSummary currentPage={1} totalPages={1} totalItems={0} pageSize={20} />
    );

    act(() => {
      vi.runAllTimers();
    });

    expect(screen.getByText(/No results match your search/i)).toBeInTheDocument();
    expect(screen.queryByText(/To start a new search/i)).not.toBeInTheDocument();
  });

  it('renders correct summary for page 1', () => {
    render(
      <AccessibleResultsSummary currentPage={1} totalPages={5} totalItems={100} pageSize={20} />
    );

    act(() => {
      vi.runAllTimers();
    });

    expect(screen.getByText('Page 1 of 5. Results 1 to 20 of 100.')).toBeInTheDocument();
  });

  it('renders correct summary for last page', () => {
    render(
      <AccessibleResultsSummary currentPage={5} totalPages={5} totalItems={98} pageSize={20} />
    );

    act(() => {
      vi.runAllTimers();
    });

    expect(screen.getByText('Page 5 of 5. Results 81 to 98 of 98.')).toBeInTheDocument();
  });

  it('has correct ARIA attributes', () => {
    render(
      <AccessibleResultsSummary currentPage={2} totalPages={3} totalItems={50} pageSize={20} />
    );
    const region = screen.getByRole('region', { name: /search results summary/i });
    expect(region).toHaveAttribute('aria-label', 'Search results summary');
    expect(region).toHaveAttribute('aria-live', 'polite');
  });

  it('forwards ref to the div', () => {
    const ref = createRef<HTMLDivElement>();

    render(
      <AccessibleResultsSummary
        ref={ref}
        currentPage={1}
        totalPages={1}
        totalItems={1}
        pageSize={20}
      />
    );

    act(() => {
      vi.runAllTimers();
    });

    expect(ref.current).toBeInstanceOf(HTMLDivElement);
    expect(ref.current?.textContent).toMatch(/page 1 of 1/i);
  });

  it('spreads additional props onto the div', () => {
    render(
      <AccessibleResultsSummary
        currentPage={1}
        totalPages={1}
        totalItems={10}
        pageSize={10}
        data-testid="summary"
        id="my-summary"
      />
    );

    const region = screen.getByTestId('summary');

    expect(region).toHaveAttribute('id', 'my-summary');
  });

  it('updates the text content when props change (simulates aria-live)', () => {
    const { rerender } = render(
      <AccessibleResultsSummary currentPage={1} totalPages={2} totalItems={25} pageSize={20} />
    );

    act(() => {
      vi.runAllTimers();
    });

    expect(screen.getByText('Page 1 of 2. Results 1 to 20 of 25.')).toBeInTheDocument();

    rerender(
      <AccessibleResultsSummary currentPage={2} totalPages={2} totalItems={25} pageSize={20} />
    );

    act(() => {
      vi.runAllTimers();
    });

    expect(screen.getByText('Page 2 of 2. Results 21 to 25 of 25.')).toBeInTheDocument();
  });

  it('renders guidance in sr-only span when addSearchGuidance is true', () => {
    render(
      <AccessibleResultsSummary
        currentPage={1}
        totalPages={1}
        totalItems={1}
        pageSize={20}
        addSearchGuidance
      />
    );

    act(() => {
      vi.runAllTimers();
    });

    expect(screen.getByText('Page 1 of 1. Results 1 to 1 of 1.')).toBeInTheDocument();

    const srOnly = screen.getByText(/To start a new search navigate to the search input above/i);
    expect(srOnly).toBeInTheDocument();
    expect(srOnly).toHaveClass('sr-only');
  });

  it('guidance disappears when addSearchGuidance toggles off', () => {
    const { rerender } = render(
      <AccessibleResultsSummary
        currentPage={1}
        totalPages={1}
        totalItems={1}
        pageSize={20}
        addSearchGuidance
      />
    );

    act(() => {
      vi.runAllTimers();
    });

    expect(
      screen.getByText(/To start a new search navigate to the search input above/i)
    ).toBeInTheDocument();

    rerender(
      <AccessibleResultsSummary
        currentPage={1}
        totalPages={1}
        totalItems={1}
        pageSize={20}
        addSearchGuidance={false}
      />
    );

    act(() => {
      vi.runAllTimers();
    });

    expect(
      screen.queryByText(/To start a new search navigate to the search input above/i)
    ).not.toBeInTheDocument();
  });
});
