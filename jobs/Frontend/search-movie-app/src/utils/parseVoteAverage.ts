export const parseVoteAverage = (voteAverage: number): { vote: number } => {
  const voteFormatted = voteAverage ? parseFloat(voteAverage.toFixed(2)) : voteAverage;

  return { vote: voteFormatted };
};
