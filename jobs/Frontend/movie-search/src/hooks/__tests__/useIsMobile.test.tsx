import { renderHook, act } from "@testing-library/react";
import { describe, it, expect, beforeEach, afterEach, vi } from "vitest";
import useIsMobile from "../useIsMobile";

function setWindowWidth(width: number) {
  Object.defineProperty(window, "innerWidth", {
    writable: true,
    configurable: true,
    value: width,
  });
}

describe("useIsMobile", () => {
  let originalMatchMedia: typeof window.matchMedia;

  beforeEach(() => {
    originalMatchMedia = window.matchMedia;
    window.matchMedia = (query: string) => {
      return {
        matches: eval(query.match(/\d+/)?.[0] ?? "0") >= window.innerWidth,
        media: query,
        onchange: null,
        addEventListener: vi.fn(),
        removeEventListener: vi.fn(),
        addListener: vi.fn(),
        removeListener: vi.fn(),
        dispatchEvent: vi.fn(),
      } as any;
    };
  });

  afterEach(() => {
    window.matchMedia = originalMatchMedia;
  });

  it("returns true if window width is less than breakpoint", () => {
    setWindowWidth(500);
    const { result } = renderHook(() => useIsMobile(600));
    expect(result.current).toBe(true);
  });

  it("returns false if window width is greater than or equal to breakpoint", () => {
    setWindowWidth(700);
    const { result } = renderHook(() => useIsMobile(600));
    expect(result.current).toBe(false);
  });

  it("updates when the window is resized", () => {
    let handler: ((e: any) => void) | undefined;
    window.matchMedia = (query: string) => {
      return {
        matches: window.innerWidth < parseInt(query.match(/\d+/)?.[0] ?? "0"),
        media: query,
        onchange: null,
        addEventListener: (event: string, cb: (e: any) => void) => {
          if (event === "change") handler = cb;
        },
        removeEventListener: vi.fn(),
        addListener: vi.fn(),
        removeListener: vi.fn(),
        dispatchEvent: vi.fn(),
      } as any;
    };
    setWindowWidth(700);
    const { result } = renderHook(() => useIsMobile(600));
    expect(result.current).toBe(false);
    setWindowWidth(500);
    act(() => {
      handler && handler({ matches: true });
    });
    expect(result.current).toBe(true);
  });
});
