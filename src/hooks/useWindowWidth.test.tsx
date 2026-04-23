import { renderHook } from '@testing-library/react-hooks';
import { useWindowWidth } from './useWindowWidth';

function fireResize(width: number) {
  window.innerWidth = width;
  window.dispatchEvent(new Event('resize'));
}

describe('useWindowWidth', () => {
  it('returns the correct device type for various window widths', async () => {
    const { result, waitFor } = renderHook(() => useWindowWidth());
    // Initially, it should be 'desktop' or undefined
    expect(result.current.isDesktop).toBe(true);
    await waitFor(() => {
      fireResize(500);
    });
    global.innerWidth = 500;
    window.dispatchEvent(new Event('resize'));

    expect(result.current.isMobile).toBe(true);
    expect(result.current.isTablet).toBe(false);
    expect(result.current.isDesktop).toBe(false);

    global.innerWidth = 800;
    window.dispatchEvent(new Event('resize'));

    expect(result.current.isMobile).toBe(false);
    expect(result.current.isTablet).toBe(true);
    expect(result.current.isDesktop).toBe(false);

    // Simulate a window width change to desktop
    global.innerWidth = 1200;
    window.dispatchEvent(new Event('resize'));

    expect(result.current.isMobile).toBe(false);
    expect(result.current.isTablet).toBe(false);
    expect(result.current.isDesktop).toBe(true);
  });
});
