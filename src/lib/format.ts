export function formatDate(dateStr: string) {
  const date = new Date(dateStr);
  return date.toLocaleDateString('en-GB', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
  });
}

export function formatVote(vote: number) {
  return Number.isFinite(vote) ? `${Math.round(vote)}%` : 'Unknown';
}

export function formatVoteFromSearch(vote: number, numberOfVotes: number) {
  return Number.isFinite(vote) && numberOfVotes > 0 ? `${Math.round(vote * 10)}%` : 'no votes';
}

export function formatPostImageAlt(movieTitle: string) {
  return `Poster for ${movieTitle || 'movie'}`;
}

export function formatRuntime(time: number) {
  return Number.isFinite(time) && time > 0 ? `${time} mins` : 'Unknow';
}
