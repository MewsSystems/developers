import React from 'react';
import { render, screen } from '@testing-library/react';
import { LargeMoviePoster } from './LargeMoviePoster';
import type { DetailedPosterUrl } from '@/types/api';

vi.mock('react-icons/ai', () => ({
  AiOutlineFileImage: (props: React.SVGProps<SVGSVGElement>) => (
    <svg data-testid="poster-fallback-icon" {...props} />
  ),
}));

describe('LargeMoviePoster', () => {
  const altText = 'Some poster alt';

  it('renders correctly with all images', () => {
    const posterUrl: DetailedPosterUrl = {
      default: '/default.jpg',
      sm: '/sm.jpg',
      lg: '/lg.jpg',
    };

    render(<LargeMoviePoster posterUrl={posterUrl} alt={altText} />);

    const img = screen.getByRole('img');
    expect(img).toHaveAttribute('src', '/default.jpg');
    expect(img).toHaveAttribute('alt', altText);

    const picture = img.parentElement as HTMLElement;
    expect(picture.tagName).toBe('PICTURE');

    const sources = Array.from(picture.querySelectorAll('source'));

    expect(sources).toHaveLength(2);

    expect(sources[0]).toHaveAttribute('media', '(min-width: 1024px)');
    expect(sources[0]).toHaveAttribute('srcset', '/lg.jpg');
    expect(sources[1]).toHaveAttribute('media', '(min-width: 640px)');
    expect(sources[1]).toHaveAttribute('srcset', '/sm.jpg');
  });

  it('renders icon fallback when no images', () => {
    render(<LargeMoviePoster posterUrl={{ default: null, sm: null, lg: null }} alt={altText} />);

    expect(screen.queryByRole('img')).not.toBeInTheDocument();
    expect(screen.getByTestId('poster-fallback-icon')).toBeInTheDocument();
  });

  it('shows correct image for only default', () => {
    render(
      <LargeMoviePoster posterUrl={{ default: '/default.jpg', sm: null, lg: null }} alt={altText} />
    );
    expect(screen.getByRole('img')).toHaveAttribute('src', '/default.jpg');

    const picture = screen.getByRole('img').parentElement as HTMLElement;
    expect(picture.querySelectorAll('source')).toHaveLength(0);
  });

  it('shows correct <source> for only sm', () => {
    render(
      <LargeMoviePoster posterUrl={{ default: null, sm: '/sm.jpg', lg: null }} alt={altText} />
    );

    expect(screen.getByRole('img')).not.toHaveAttribute('src');

    const picture = screen.getByRole('img').parentElement as HTMLElement;

    expect(picture.querySelector('source[media="(min-width: 640px)"]')).toHaveAttribute(
      'srcset',
      '/sm.jpg'
    );
    expect(picture.querySelectorAll('source')).toHaveLength(1);
  });

  it('shows correct <source> for only lg', () => {
    render(
      <LargeMoviePoster posterUrl={{ default: null, sm: null, lg: '/lg.jpg' }} alt={altText} />
    );
    screen.debug();

    expect(screen.getByRole('img')).not.toHaveAttribute('src');

    const picture = screen.getByRole('img').parentElement as HTMLElement;

    expect(picture.querySelector('source[media="(min-width: 1024px)"]')).toHaveAttribute(
      'srcset',
      '/lg.jpg'
    );
    expect(picture.querySelectorAll('source')).toHaveLength(1);
  });

  it('always passes accessibility alt', () => {
    const posterUrl: DetailedPosterUrl = {
      default: '/default.jpg',
      sm: '/sm.jpg',
      lg: '/lg.jpg',
    };
    render(<LargeMoviePoster posterUrl={posterUrl} alt={altText} />);

    expect(screen.getByAltText(altText)).toBeInTheDocument();
  });

  it('always sets correct fetchPriority', () => {
    const posterUrl: DetailedPosterUrl = {
      default: '/default.jpg',
      sm: '/sm.jpg',
      lg: '/lg.jpg',
    };
    render(<LargeMoviePoster posterUrl={posterUrl} alt={altText} />);

    expect(screen.getByRole('img')).toHaveAttribute('fetchpriority', 'high');
  });
});
