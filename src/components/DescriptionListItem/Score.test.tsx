import { render, screen } from '@testing-library/react';
import { Score } from './Score';
import { formatVote } from '@/lib/format';

describe('<Score />', () => {
  const score = 8.5;
  const count = 200;
  const formatted = formatVote(score, count);

  it('renders the score and vote count formatted', () => {
    render(
      <dl>
        <Score score={score} count={count} />
      </dl>
    );

    const term = screen.getByText(/score:/i);
    const detail = screen.getByText(formatted);

    expect(term.tagName).toBe('DT');
    expect(detail.tagName).toBe('DD');
    expect(detail).toHaveTextContent(formatted);
  });

  it('applies small text styles when isSmall is true', () => {
    render(
      <dl>
        <Score score={score} count={count} isSmall />
      </dl>
    );

    const term = screen.getByText(/score:/i);
    const detail = screen.getByText(formatted).closest('dd');

    expect(term).toHaveClass('text-sm');
    expect(term).toHaveClass('font-normal');
    expect(detail).toHaveClass('text-sm');
    expect(detail).toHaveClass('text-stone-800');
  });
});
