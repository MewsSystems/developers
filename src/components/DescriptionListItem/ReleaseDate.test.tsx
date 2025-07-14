import { render, screen } from '@testing-library/react';
import { ReleaseDate } from './ReleaseDate';
import { formatDate } from '@/lib/format';

describe('<ReleaseDate />', () => {
  const validDate = '2024-12-25';
  const formatted = formatDate(validDate);

  it('renders a formatted date inside a <time> element', () => {
    render(
      <dl>
        <ReleaseDate date={validDate} />
      </dl>
    );

    const timeEl = screen.getByText(formatted);
    expect(timeEl).toBeInTheDocument();
    expect(timeEl.tagName).toBe('TIME');
    expect(timeEl).toHaveAttribute('dateTime', validDate);
  });

  it('renders "unknown" when date is empty', () => {
    render(
      <dl>
        <ReleaseDate date="" />
      </dl>
    );

    expect(screen.getByText(/unknown/i)).toBeInTheDocument();
  });

  it('applies small text classes when isSmall is true', () => {
    render(
      <dl>
        <ReleaseDate date={validDate} isSmall />
      </dl>
    );

    const term = screen.getByText('Released:');
    const timeEl = screen.getByText(formatDate(validDate));
    const detail = timeEl.closest('dd');

    expect(term).toHaveClass('text-sm');
    expect(term).toHaveClass('font-normal');
    expect(detail).toHaveClass('text-sm');
    expect(detail).toHaveClass('text-stone-800');
  });
});
