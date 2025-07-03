import { z } from 'zod';

export const movieIdSlugSchema = z.string().regex(/^(\d+)-.+$/, 'Invalid movie slug format');

export function createMovieSlug(id: number, originalTitle: string): string {
  return (
    id +
    '-' +
    originalTitle
      // Replace spaces (including full-width) with hyphens
      .replace(/[\s\u3000]+/g, '-')
      // Remove forbidden/special URL characters (you can add more as needed)
      .replace(/[:\/\\?%#\[\]@!$&'()*+,;=<>"]/g, '')
      .toLowerCase()
      // Remove repeated hyphens
      .replace(/-+/g, '-')
      // Remove leading/trailing hyphens
      .replace(/^-+|-+$/g, '')
  );
}
