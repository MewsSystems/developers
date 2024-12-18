// Utility to convert an array of values or objects into a comma-separated string
export const arrayToString = <T,>(array?: T[], key?: keyof T): string => {
	if (!array || array.length === 0) return "N/A"; // Handle empty or undefined array

	if (key) {
		// Map array of objects based on the key and join values
		return array
			.map((item) => (item[key] !== undefined ? String(item[key]) : ""))
			.filter((val) => val) // Filter out empty strings
			.join(", ");
	}

	// If no key is provided, assume it's an array of strings/numbers
	return array.map((item) => String(item)).join(", ");
};
