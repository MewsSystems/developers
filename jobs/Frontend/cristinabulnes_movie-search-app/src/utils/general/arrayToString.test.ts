import arrayToString from "./";

describe("arrayToString utility", () => {
	it("should return a comma-separated string from an array of objects", () => {
		const genres = [
			{ name: "Action" },
			{ name: "Adventure" },
			{ name: "Sci-Fi" },
		];
		expect(arrayToString(genres, "name")).toBe("Action, Adventure, Sci-Fi");
	});

	it("should return a comma-separated string from an array of strings", () => {
		const fruits = ["Apple", "Banana", "Cherry"];
		expect(arrayToString(fruits)).toBe("Apple, Banana, Cherry");
	});

	it("should return a comma-separated string from an array of numbers", () => {
		const numbers = [1, 2, 3];
		expect(arrayToString(numbers)).toBe("1, 2, 3");
	});

	it("should return 'N/A' for an empty array", () => {
		expect(arrayToString([])).toBe("N/A");
	});

	it("should return 'N/A' for an undefined array", () => {
		expect(arrayToString(undefined)).toBe("N/A");
	});

	it("should ignore undefined or missing key values in objects", () => {
		const items = [{ name: "Action" }, {}, { name: "Sci-Fi" }];
		expect(arrayToString(items, "name")).toBe("Action, Sci-Fi");
	});
});
