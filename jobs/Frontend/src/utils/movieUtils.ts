export const displayRatings = (voteAverage: number, voteCount: number) => {
    const displayRatingPercent = `${Math.round(voteAverage * 10)}%`;
    const displayRatingCount = new Intl.NumberFormat('en-US').format(voteCount);
    return `${displayRatingPercent} from ${displayRatingCount} ratings`;
};

export const displayRuntime = (runtime: number) => runtime > 60
    ? `${Math.floor(runtime / 60)}h ${runtime % 60}m`
    : `${runtime}m`;