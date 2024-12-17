import axios from "axios";
import apiClient from "./apiClient";

// Mocking the axios module
jest.mock("axios", () => {
	// Create a mock of the AxiosInstance
	const mockedAxiosInstance = {
		get: jest.fn(),
	};

	// Mock axios.create to return the mockedAxiosInstance
	return {
		create: jest.fn(() => mockedAxiosInstance),
	};
});

describe("API Service", () => {
	it("should make a GET request using the axios instance", async () => {
		const mockResponse = { data: { message: "success" } };

		// Mock axiosInstance.get to resolve with mock response
		(axios.create().get as jest.Mock).mockResolvedValue(mockResponse);

		const response = await apiClient.get("/test-endpoint");

		// Ensure the response matches the mock response
		expect(response).toEqual(mockResponse);
		// Check if axios.create().get was called with the correct URL
		expect(axios.create().get).toHaveBeenCalledWith("/test-endpoint", {});
	});

	it("should pass configuration parameters to axios", async () => {
		const mockResponse = { data: { message: "success" } };
		const config = { headers: { Authorization: "Bearer token" } };

		// Mock axiosInstance.get to resolve with mock response
		(axios.create().get as jest.Mock).mockResolvedValue(mockResponse);

		const response = await apiClient.get("/test-endpoint", config);

		// Ensure the response matches the mock response
		expect(response).toEqual(mockResponse);
		// Check if axios.create().get was called with the correct URL and config
		expect(axios.create().get).toHaveBeenCalledWith("/test-endpoint", config);
	});

	it("should handle errors in the GET request", async () => {
		const errorMessage = "Something went wrong";

		// Mock axiosInstance.get to reject with an error
		(axios.create().get as jest.Mock).mockRejectedValue(
			new Error(errorMessage)
		);

		try {
			await apiClient.get("/test-endpoint");
		} catch (error: any) {
			expect(error.message).toBe(errorMessage);
		}
	});
});
