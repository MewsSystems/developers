import '@testing-library/jest-dom';
import { vi } from 'vitest';

//for pagination tests mock window.scrollTo
Object.defineProperty(global, 'scrollTo', {
  value: vi.fn(),
  writable: true,
});