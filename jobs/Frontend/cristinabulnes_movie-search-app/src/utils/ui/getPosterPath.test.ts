import { BASE_IMAGE_URL, getPosterPath } from "./getPosterPath";

describe("getPosterPath", () => {
	it("should return the full poster path for a valid posterPath", () => {
		const posterPath = "/sample.jpg";
		const width = 300;
		const result = getPosterPath(posterPath, width);
		expect(result).toBe(`${BASE_IMAGE_URL}${width}${posterPath}`);
	});

	it("should return the placeholder image URL when posterPath is null", () => {
		const width = 300;
		const result = getPosterPath(null, width);
		expect(result).toBe("https://via.placeholder.com/300x450?text=No+Image");
	});

	it("should default to width 300 when width is not provided", () => {
		const posterPath = "/sample.jpg";
		const result = getPosterPath(posterPath);
		expect(result).toBe(`${BASE_IMAGE_URL}300${posterPath}`);
	});
});
