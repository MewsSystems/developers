import { describe, expect, it } from 'vitest';

import { generatePageNumbers } from './generatePageNumbers';

describe('generatePageNumbers util', () => {
  it('generates an empty array for 0 pages', () => {
    expect(generatePageNumbers(0)).toEqual([]);
  });

  it('generates an array of 1 number for 1 page', () => {
    expect(generatePageNumbers(1)).toEqual([1]);
  });

  it('generates an array of numbers from 1 to n for n pages', () => {
    const numberOfPages = 5;
    const expectedArray = [1, 2, 3, 4, 5];
    expect(generatePageNumbers(numberOfPages)).toEqual(expectedArray);
  });

  it('generates an empty array for negative numbers', () => {
    expect(generatePageNumbers(-1)).toEqual([]);
  });
});
