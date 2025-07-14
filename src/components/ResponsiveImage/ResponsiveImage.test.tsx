import { render, screen } from '@testing-library/react';
import { ResponsiveImage, type BreakpointDefinition } from './ResponsiveImage';

vi.mock('react-icons/ai', () => ({
  AiOutlineFileImage: (props: React.SVGProps<SVGSVGElement>) => (
    <svg data-testid="poster-fallback-icon" {...props} />
  ),
}));

describe('ResponsiveImage', () => {
  const altText = 'Some poster alt';

  it('renders with all breakpoints present', () => {
    const breakpointDefinition: BreakpointDefinition = {
      default: { src: '/default.jpg', containerSize: 'w-[100px] h-[100px]' },
      sm: { src: '/sm.jpg', containerSize: 'sm:w-[200px] sm:h-[200px]' },
      md: { src: '/md.jpg', containerSize: 'md:w-[300px] md:h-[300px]' },
      lg: { src: '/lg.jpg', containerSize: 'lg:w-[400px] lg:h-[400px]' },
      xl: { src: '/xl.jpg', containerSize: 'xl:w-[500px] xl:h-[500px]' },
      '2xl': { src: '/2xl.jpg', containerSize: '2xl:w-[600px] 2xl:h-[600px]' },
    };

    render(
      <ResponsiveImage
        breakpointDefinition={breakpointDefinition}
        alt={altText}
        width={100}
        height={100}
        fetchPriority="auto"
      />
    );

    const img = screen.getByRole('img');
    expect(img).toHaveAttribute('src', '/default.jpg');
    expect(img).toHaveAttribute('alt', altText);

    const sources = screen.getByRole('img').parentElement!.querySelectorAll('source');
    expect(sources).toHaveLength(5);

    const mediaOrder = [
      '(min-width: 1536px)', // 2xl
      '(min-width: 1280px)', // xl
      '(min-width: 1024px)', // lg
      '(min-width: 768px)', // md
      '(min-width: 640px)', // sm
    ];
    mediaOrder.forEach((media, idx) => {
      expect(sources[idx]).toHaveAttribute('media', media);
    });

    expect(sources[0]).toHaveAttribute('srcset', '/2xl.jpg');
    expect(sources[1]).toHaveAttribute('srcset', '/xl.jpg');
    expect(sources[2]).toHaveAttribute('srcset', '/lg.jpg');
    expect(sources[3]).toHaveAttribute('srcset', '/md.jpg');
    expect(sources[4]).toHaveAttribute('srcset', '/sm.jpg');

    const fallbackIcon = screen.queryByTestId('poster-fallback-icon');

    expect(fallbackIcon).toHaveClass('hidden');
    expect(fallbackIcon).toHaveClass('sm:hidden');
    expect(fallbackIcon).toHaveClass('md:hidden');
    expect(fallbackIcon).toHaveClass('lg:hidden');
    expect(fallbackIcon).toHaveClass('xl:hidden');
    expect(fallbackIcon).toHaveClass('2xl:hidden');
  });

  it('renders fallback icon if all src are null', () => {
    const breakpointDefinition: BreakpointDefinition = {
      default: { src: null, containerSize: 'w-[100px] h-[100px]' },
      sm: { src: null, containerSize: 'sm:w-[200px] sm:h-[200px]' },
    };

    render(
      <ResponsiveImage
        breakpointDefinition={breakpointDefinition}
        alt={altText}
        width={100}
        height={100}
      />
    );

    expect(screen.queryByRole('img')).not.toBeInTheDocument();
    expect(screen.getByTestId('poster-fallback-icon')).toBeInTheDocument();
  });

  it('renders only the source and <img> for present breakpoints', () => {
    const breakpointDefinition: BreakpointDefinition = {
      default: { src: null, containerSize: 'w-[100px] h-[100px]' },
      sm: { src: '/sm.jpg', containerSize: 'sm:w-[200px] sm:h-[200px]' },
      md: { src: null, containerSize: 'md:w-[300px] md:h-[300px]' },
      lg: { src: null, containerSize: 'lg:w-[400px] lg:h-[400px]' },
    };

    render(
      <ResponsiveImage
        breakpointDefinition={breakpointDefinition}
        alt={altText}
        width={100}
        height={100}
      />
    );
    const sources = screen.getByRole('img').parentElement!.querySelectorAll('source');

    expect(sources).toHaveLength(1);
    expect(sources[0]).toHaveAttribute('srcset', '/sm.jpg');
    expect(sources[0]).toHaveAttribute('media', '(min-width: 640px)');
  });

  it('uses correct fetchPriority', () => {
    const breakpointDefinition: BreakpointDefinition = {
      default: { src: '/default.jpg', containerSize: 'w-[100px] h-[100px]' },
    };
    render(
      <ResponsiveImage
        breakpointDefinition={breakpointDefinition}
        alt={altText}
        width={100}
        height={100}
        fetchPriority="high"
      />
    );
    expect(screen.getByRole('img')).toHaveAttribute('fetchpriority', 'high');
  });

  it('applies alt text for accessibility', () => {
    const breakpointDefinition: BreakpointDefinition = {
      default: { src: '/default.jpg', containerSize: 'w-[100px] h-[100px]' },
    };
    render(
      <ResponsiveImage
        breakpointDefinition={breakpointDefinition}
        alt={altText}
        width={100}
        height={100}
      />
    );
    expect(screen.getByAltText(altText)).toBeInTheDocument();
  });

  it('renders only default image and no <source> if only default src is present', () => {
    const breakpointDefinition: BreakpointDefinition = {
      default: { src: '/default.jpg', containerSize: 'w-[100px] h-[100px]' },
      sm: { src: null, containerSize: 'sm:w-[200px] sm:h-[200px]' },
    };
    render(
      <ResponsiveImage
        breakpointDefinition={breakpointDefinition}
        alt={altText}
        width={100}
        height={100}
      />
    );

    expect(screen.getByRole('img')).toHaveAttribute('src', '/default.jpg');
    const sources = screen.getByRole('img').parentElement!.querySelectorAll('source');
    expect(sources).toHaveLength(0);
  });

  it('orders source elements according to BREAKPOINTS', () => {
    const breakpointDefinition: BreakpointDefinition = {
      default: { src: '/default.jpg', containerSize: 'w-[100px] h-[100px]' },
      md: { src: '/md.jpg', containerSize: 'md:w-[300px] md:h-[300px]' },
      lg: { src: '/lg.jpg', containerSize: 'lg:w-[400px] lg:h-[400px]' },
      sm: { src: '/sm.jpg', containerSize: 'sm:w-[200px] sm:h-[200px]' },
    };

    render(
      <ResponsiveImage
        breakpointDefinition={breakpointDefinition}
        alt={altText}
        width={100}
        height={100}
      />
    );
    const sources = screen.getByRole('img').parentElement!.querySelectorAll('source');

    expect(sources[0]).toHaveAttribute('media', '(min-width: 1024px)');
    expect(sources[1]).toHaveAttribute('media', '(min-width: 768px)');
    expect(sources[2]).toHaveAttribute('media', '(min-width: 640px)');
  });

  it('renders <img> without src if default is null but another breakpoint has an image', () => {
    const breakpointDefinition: BreakpointDefinition = {
      default: { src: null, containerSize: 'w-[100px] h-[100px]' },
      md: { src: '/md.jpg', containerSize: 'md:w-[300px] md:h-[300px]' },
    };

    render(
      <ResponsiveImage
        breakpointDefinition={breakpointDefinition}
        alt={altText}
        width={100}
        height={100}
      />
    );
    const img = screen.getByRole('img');
    expect(img).not.toHaveAttribute('src');
    const picture = img.parentElement as HTMLElement;
    expect(picture.querySelector('source[media="(min-width: 768px)"]')).toHaveAttribute(
      'srcset',
      '/md.jpg'
    );
  });
});
