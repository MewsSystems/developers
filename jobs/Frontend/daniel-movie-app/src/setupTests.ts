import '@testing-library/jest-dom';
import { TextEncoder, TextDecoder } from 'util';

global.TextEncoder = TextEncoder;
global.TextDecoder = TextDecoder;

// Bypass "Could not parse CSS stylesheet" errors produced by stylesheets using (legitimate) 
// features that the jsdom CSS parser does not support
const originalConsoleError = console.error;
console.error = function (...data) {
  if (
    typeof data[0]?.toString === 'function' && 
    data[0].toString().includes('Error: Could not parse CSS stylesheet')
  ) return;
  originalConsoleError(...data);
};
