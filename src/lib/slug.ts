export function createMovieSlug(id: number, originalTitle: string): string {
  return `${id}-${encodeURIComponent(originalTitle.replace(/\s+/g, '-').toLowerCase())}`;
}
