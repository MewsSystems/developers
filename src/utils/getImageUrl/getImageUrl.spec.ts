import {IMAGE_BASE_URL} from '../../api/movieApi/constants';
import {getImageUrl} from './getImageUrl';
import type {ImageSize} from './types';

describe('getImageUrl', () => {
  const testPath = '/test-image.jpg';
  const sizes: ImageSize[] = ['w500', 'original'];

  it('should return null for null path', () => {
    expect(getImageUrl(null)).toBeNull();
  });

  sizes.forEach((size) => {
    it(`should construct correct URL with size ${size}`, () => {
      const expected = `${IMAGE_BASE_URL}/${size}${testPath}`;
      expect(getImageUrl(testPath, size)).toBe(expected);
    });
  });

  it('should use w500 as default size', () => {
    const expected = `${IMAGE_BASE_URL}/w500${testPath}`;
    expect(getImageUrl(testPath)).toBe(expected);
  });

  it('should handle paths without leading slash', () => {
    const pathWithoutSlash = 'test-image.jpg';
    const expected = `${IMAGE_BASE_URL}/w500/${pathWithoutSlash}`;
    expect(getImageUrl(pathWithoutSlash)).toBe(expected);
  });

  it('should return original URL if path is already a full URL', () => {
    const fullUrl = 'https://example.com/image.jpg';
    expect(getImageUrl(fullUrl)).toBe(fullUrl);
  });
});
