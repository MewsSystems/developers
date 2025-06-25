export function formatDate(date: string) {
  return date ? new Date(date).toLocaleDateString('en-GB') : 'Unknown';
}

export function formatVote(vote: number) {
  return Number.isFinite(vote) ? `${Math.round(vote)}%` : 'Unknown';
}

export function formatVoteFromSearch(vote: number) {
  return Number.isFinite(vote) ? `${Math.round(vote * 10)}%` : 'Unknown';
}

export function formatPostImageAlt(movieTitle: string) {
  return `Poster for ${movieTitle || 'movie'}`;
}

export function formatRuntime(time: number) {
  return Number.isFinite(time) && time > 0 ? `${time} mins` : 'Unknow';
}
