import { render, screen } from '@testing-library/react';
import { DescriptionListItem } from './DescriptionListItem';

describe('<DescriptionListItem />', () => {
  it('renders the term and detail correctly', () => {
    render(
      <dl>
        <DescriptionListItem term="Director" detail="Christopher Nolan" />
      </dl>
    );

    const term = screen.getByText('Director');
    const detail = screen.getByText('Christopher Nolan');

    expect(term.tagName).toBe('DT');
    expect(detail.tagName).toBe('DD');
    expect(term).toBeInTheDocument();
    expect(detail).toBeInTheDocument();
  });

  it('applies custom class names to term and detail', () => {
    render(
      <dl>
        <DescriptionListItem
          term="Rating"
          detail="PG-13"
          termClassName="custom-term"
          detailClassName="custom-detail"
        />
      </dl>
    );

    const term = screen.getByText('Rating');
    const detail = screen.getByText('PG-13');

    expect(term).toHaveClass('custom-term');
    expect(detail).toHaveClass('custom-detail');
  });

  it('renders ReactNode as detail', () => {
    render(
      <dl>
        <DescriptionListItem term="More Info" detail={<a href="/movie">View</a>} />
      </dl>
    );

    const link = screen.getByRole('link', { name: /view/i });
    expect(link).toBeInTheDocument();
    expect(link).toHaveAttribute('href', '/movie');
  });
});
