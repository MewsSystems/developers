const getRatingCategory = (voteFormatted: number): 'low' | 'medium' | 'high' => {
  if (voteFormatted < 4) return 'low';
  if (voteFormatted >= 4 && voteFormatted <= 7) return 'medium';
  return 'high';
};

const parseVoteAverage = (voteAverage: number): { vote: number; type: string } => {
  const voteFormatted = parseFloat(voteAverage.toFixed(2));

  return { vote: voteFormatted, type: getRatingCategory(voteFormatted) };
};

export { parseVoteAverage };
