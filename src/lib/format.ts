export function formatDate(date: string) {
  return date ? new Date(date).toLocaleDateString('en-GB') : 'Unknown';
}

export function formatVote(vote: number) {
  return `${Math.round(vote * 10)}%`;
}
