import { Component, ReactNode } from "react";
import ErrorComponent from "../Error/Error";

interface ErrorBoundaryProps {
	children: ReactNode;
	fallback?: ReactNode;
}

interface ErrorBoundaryState {
	hasError: boolean;
}

class ErrorBoundary extends Component<ErrorBoundaryProps, ErrorBoundaryState> {
	constructor(props: ErrorBoundaryProps) {
		super(props);
		this.state = { hasError: false };
	}

	static getDerivedStateFromError(): ErrorBoundaryState {
		// Update state so the next render will show the fallback UI.
		return { hasError: true };
	}

	componentDidCatch(error: Error, info: React.ErrorInfo): void {
		console.error("Error caught by ErrorBoundary:", error);
		console.error(info);
	}

	render() {
		if (this.state.hasError) {
			return this.props.fallback ? (
				this.props.fallback
			) : (
				<ErrorComponent message="An unexpected error occurred!" />
			);
		}

		return this.props.children;
	}
}

export default ErrorBoundary;
