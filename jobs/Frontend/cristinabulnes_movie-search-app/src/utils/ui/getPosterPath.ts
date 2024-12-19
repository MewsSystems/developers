export const BASE_IMAGE_URL = "https://image.tmdb.org/t/p/w";

// Utility to create the full poster path or return a placeholder if unavailable
export const getPosterPath = (
	posterPath: string | null,
	width: number = 300
): string => {
	if (posterPath) {
		return `${BASE_IMAGE_URL}${width}${posterPath}`;
	}

	// Default placeholder image when posterPath is null or undefined
	return `https://via.placeholder.com/${width}x${Math.round(
		(width / 2) * 3
	)}?text=No+Image`;
};
