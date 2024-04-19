export const displayRatings = (voteAverage: number, voteCount: number) => {
    const displayRatingPercent = `${Math.round(voteAverage * 10)}%`;
    const displayRatingCount = new Intl.NumberFormat('en-US').format(voteCount);
    return `${displayRatingPercent} from ${displayRatingCount} ratings`;
};

export const displayRuntime = (runtime: number) => {
    if (runtime < 60) {
        return `${runtime}m`;
    }

    const displayHours = `${Math.floor(runtime / 60)}h`;

    return runtime % 60 === 0
        ? displayHours
        : `${displayHours} ${runtime % 60}m`;
};