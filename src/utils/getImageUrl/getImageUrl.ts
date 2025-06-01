import {IMAGE_BASE_URL} from '../../api/movieApi/constants';
import type {ImageSize} from './types';

export const getImageUrl = (path: string | null, size: ImageSize = 'w500'): string | null => {
  if (!path) return null;

  if (path.startsWith('http://') || path.startsWith('https://')) {
    return path;
  }

  const normalizedPath = path.startsWith('/') ? path : `/${path}`;

  return `${IMAGE_BASE_URL}/${size}${normalizedPath}`;
};
