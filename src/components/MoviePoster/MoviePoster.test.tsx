import { render, screen } from '@testing-library/react';
import { MoviePoster } from './MoviePoster';
import type { PosterUrl } from '@/types/api';

vi.mock('react-icons/ai', () => ({
  AiOutlineFileImage: (props: React.SVGProps<SVGSVGElement>) => (
    <svg data-testid="poster-fallback-icon" {...props} />
  ),
}));

describe('MoviePoster', () => {
  const altText = 'Poster alt text';

  it('renders correctly with all images and sources are in correct order', () => {
    const posterUrl: PosterUrl = {
      default: '/default.jpg',
      sm: '/sm.jpg',
      md: '/md.jpg',
    };

    render(<MoviePoster posterUrl={posterUrl} alt={altText} />);
    const img = screen.getByRole('img');
    expect(img).toHaveAttribute('src', '/default.jpg');
    expect(img).toHaveAttribute('alt', altText);

    expect(img.parentElement?.tagName).toBe('PICTURE');

    const picture = img.parentElement as HTMLElement;
    const sources = Array.from(picture.querySelectorAll('source'));

    expect(sources).toHaveLength(2);

    expect(sources[0]).toHaveAttribute('media', '(min-width: 768px)');
    expect(sources[0]).toHaveAttribute('srcset', '/md.jpg');
    expect(sources[1]).toHaveAttribute('media', '(min-width: 640px)');
    expect(sources[1]).toHaveAttribute('srcset', '/sm.jpg');
  });

  it('renders icon fallback when no images', () => {
    render(<MoviePoster posterUrl={{ default: null, sm: null, md: null }} alt={altText} />);
    expect(screen.queryByRole('img')).not.toBeInTheDocument();
    expect(screen.getByTestId('poster-fallback-icon')).toBeInTheDocument();
  });

  it('shows correct image for only default', () => {
    render(
      <MoviePoster posterUrl={{ default: '/default.jpg', sm: null, md: null }} alt={altText} />
    );
    const img = screen.getByRole('img');
    expect(img).toHaveAttribute('src', '/default.jpg');
    expect(img.parentElement?.querySelectorAll('source')).toHaveLength(0);
  });

  it('shows correct <source> for only sm', () => {
    render(<MoviePoster posterUrl={{ default: null, sm: '/sm.jpg', md: null }} alt={altText} />);

    expect(screen.getByRole('img')).not.toHaveAttribute('src');

    const picture = screen.getByRole('img').parentElement as HTMLElement;
    expect(picture.querySelector('source[media="(min-width: 640px)"]')).toHaveAttribute(
      'srcset',
      '/sm.jpg'
    );
    expect(picture.querySelectorAll('source')).toHaveLength(1);
  });

  it('shows correct <source> for only md', () => {
    render(<MoviePoster posterUrl={{ default: null, sm: null, md: '/md.jpg' }} alt={altText} />);

    expect(screen.getByRole('img')).not.toHaveAttribute('src');

    const picture = screen.getByRole('img').parentElement as HTMLElement;
    expect(picture.querySelector('source[media="(min-width: 768px)"]')).toHaveAttribute(
      'srcset',
      '/md.jpg'
    );
    expect(picture.querySelectorAll('source')).toHaveLength(1);
  });

  it('always passes accessibility alt', () => {
    render(
      <MoviePoster
        posterUrl={{ default: '/default.jpg', sm: '/sm.jpg', md: '/md.jpg' }}
        alt={altText}
      />
    );
    expect(screen.getByAltText(altText)).toBeInTheDocument();
  });

  it('applies container and img classes as expected', () => {
    render(
      <MoviePoster
        posterUrl={{ default: '/default.jpg', sm: '/sm.jpg', md: '/md.jpg' }}
        alt={altText}
      />
    );
    const container = screen.getByRole('img').closest('div');

    expect(container).toHaveClass('flex');
    expect(container).toHaveClass('rounded-md');

    const img = screen.getByRole('img');

    expect(img).toHaveClass('object-contain');
    expect(img).toHaveClass('border-cyan-500');
  });
});
