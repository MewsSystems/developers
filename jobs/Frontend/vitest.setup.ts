import { vi } from "vitest";

const intersectionObserverMock = () => ({
  observe: () => null,
  disconnect: () => null,
});

window.IntersectionObserver = vi
  .fn()
  .mockImplementation(intersectionObserverMock);
