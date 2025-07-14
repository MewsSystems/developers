import { render, screen, fireEvent, act } from '@testing-library/react';
import { createRef } from 'react';
import { ResultsSummary } from './ResultsSummary';
import { AccessibleResultsSummary } from './AccessibleResultsSummary';

beforeEach(() => {
  vi.useFakeTimers();
});
afterEach(() => {
  vi.runOnlyPendingTimers();
  vi.useRealTimers();
});

describe('ResultsSummary', () => {
  it('renders children', () => {
    render(
      <ResultsSummary>
        <div data-testid="child-div">Hello</div>
      </ResultsSummary>
    );
    expect(screen.getByTestId('child-div')).toBeInTheDocument();
    expect(screen.getByText('Hello')).toBeInTheDocument();
  });

  it('adds addSearchGuidance=true to AccessibleResultsSummary when focused', async () => {
    render(
      <ResultsSummary>
        <AccessibleResultsSummary currentPage={1} totalPages={1} totalItems={1} pageSize={20} />
      </ResultsSummary>
    );

    const container = screen.getByTestId('results-summary-container');

    fireEvent.focus(container);

    await act(() => Promise.resolve()); // let React flush updates
    act(() => {
      vi.runAllTimers();
    });

    expect(
      screen.getByText(/to start a new search navigate to the search input above/i, {
        selector: '.sr-only',
      })
    ).toBeInTheDocument();
  });

  it('removes addSearchGuidance (guidance disappears) on blur', async () => {
    render(
      <ResultsSummary>
        <AccessibleResultsSummary currentPage={1} totalPages={1} totalItems={1} pageSize={20} />
      </ResultsSummary>
    );

    const container = screen.getByTestId('results-summary-container');

    fireEvent.focus(container);

    await act(() => Promise.resolve());
    act(() => {
      vi.runAllTimers();
    });

    expect(
      screen.getByText(/to start a new search navigate to the search input above/i, {
        selector: '.sr-only',
      })
    ).toBeInTheDocument();

    fireEvent.blur(container);

    await act(() => Promise.resolve());
    act(() => {
      vi.runAllTimers();
    });

    expect(
      screen.queryByText(/to start a new search navigate to the search input above/i, {
        selector: '.sr-only',
      })
    ).not.toBeInTheDocument();
  });

  it('does not inject addSearchGuidance to non-AccessibleResultsSummary children', () => {
    const CustomChild = (props: { test?: boolean }) => (
      <span data-testid="custom-child">{String(!!props.test)}</span>
    );
    render(
      <ResultsSummary>
        <CustomChild />
      </ResultsSummary>
    );
    expect(screen.getByTestId('custom-child')).toHaveTextContent('false');
  });

  it('spreads additional props onto the container', () => {
    render(
      <ResultsSummary data-testid="my-summary" id="special-id">
        <div>Test</div>
      </ResultsSummary>
    );
    const container = screen.getByTestId('my-summary');
    expect(container).toHaveAttribute('id', 'special-id');
  });

  it('forwards ref to the container div', () => {
    const ref = createRef<HTMLDivElement>();
    render(
      <ResultsSummary ref={ref}>
        <div>Something</div>
      </ResultsSummary>
    );
    expect(ref.current).toBeInstanceOf(HTMLDivElement);
    expect(ref.current).toHaveClass('min-h-[24px]');
  });

  it('applies custom className', () => {
    render(
      <ResultsSummary className="bg-cyan-100">
        <div>Test</div>
      </ResultsSummary>
    );
    const container = screen.getByTestId('results-summary-container');
    expect(container).toHaveClass('bg-cyan-100');
  });

  it('can handle multiple children and only injects addSearchGuidance to AccessibleResultsSummary', async () => {
    render(
      <ResultsSummary>
        <div data-testid="first">First</div>
        <AccessibleResultsSummary currentPage={1} totalPages={1} totalItems={1} pageSize={20} />
        <div data-testid="last">Last</div>
      </ResultsSummary>
    );

    expect(screen.getByTestId('first')).toBeInTheDocument();
    expect(screen.getByTestId('last')).toBeInTheDocument();

    const container = screen.getByTestId('results-summary-container');

    fireEvent.focus(container);

    await act(() => Promise.resolve());
    act(() => {
      vi.runAllTimers();
    });

    expect(
      screen.getByText(/to start a new search navigate to the search input above/i, {
        selector: '.sr-only',
      })
    ).toBeInTheDocument();
  });
});
