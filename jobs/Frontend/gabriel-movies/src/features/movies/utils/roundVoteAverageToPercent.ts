export const roundVoteAverageToPercent = (v: number): number =>
  Math.round((v || 0) * 10);