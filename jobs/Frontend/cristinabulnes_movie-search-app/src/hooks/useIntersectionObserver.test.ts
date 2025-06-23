import { renderHook } from "@testing-library/react";
import { useIntersectionObserver } from "./useIntersectionObserver";

describe("useIntersectionObserver", () => {
	let observe: jest.Mock;
	let disconnect: jest.Mock;

	beforeEach(() => {
		observe = jest.fn();
		disconnect = jest.fn();

		// Mock the IntersectionObserver API
		global.IntersectionObserver = jest.fn(() => ({
			observe,
			disconnect,
		})) as unknown as jest.Mock;
	});

	afterEach(() => {
		jest.clearAllMocks();
	});

	it("should call observe with the given element", () => {
		const mockCallback = jest.fn();
		const elementRef = { current: document.createElement("div") };

		renderHook(() => useIntersectionObserver(mockCallback, elementRef));

		expect(observe).toHaveBeenCalledWith(elementRef.current);
	});

	it("should disconnect the observer on unmount", () => {
		const mockCallback = jest.fn();
		const elementRef = { current: document.createElement("div") };

		const { unmount } = renderHook(() =>
			useIntersectionObserver(mockCallback, elementRef)
		);

		unmount();

		expect(disconnect).toHaveBeenCalled();
	});

	it("should call the callback when the element becomes visible", () => {
		const mockCallback = jest.fn();
		const elementRef = { current: document.createElement("div") };

		renderHook(() => useIntersectionObserver(mockCallback, elementRef));

		// Simulate an intersection
		const mockEntry = [{ isIntersecting: true }];
		const mockObserverCallback = (global.IntersectionObserver as jest.Mock).mock
			.calls[0][0];
		mockObserverCallback(mockEntry);

		expect(mockCallback).toHaveBeenCalled();
	});
});
