import { useEffect } from "react";

export const useIntersectionObserver = (
	callback: () => void,
	elementRef: React.RefObject<HTMLElement>
) => {
	useEffect(() => {
		if (!elementRef.current) return;

		const options = {
			root: null, // Use the viewport as the root
			rootMargin: "0px",
			threshold: 1.0, // Trigger when the element is fully visible
		};

		const handleIntersection = ([entry]: IntersectionObserverEntry[]) => {
			if (entry.isIntersecting) {
				callback();
			}
		};

		const observer = new IntersectionObserver(handleIntersection, options);
		observer.observe(elementRef.current);

		return () => {
			observer.disconnect();
		};
	}, [callback, elementRef]);
};
