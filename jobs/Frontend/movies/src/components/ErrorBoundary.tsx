import {
  Component,
  ComponentType,
  ErrorInfo,
  PropsWithChildren,
  createElement,
} from "react";

export type FallbackProps = { error: Error };

type ErrorBoudaryProps = PropsWithChildren<{
  fallbackComponent?: ComponentType<FallbackProps>;
}>;

type ErrorBoundaryState =
  | {
      hasError: true;
      error: Error;
    }
  | {
      hasError: false;
      error: null;
    };

const initialState: ErrorBoundaryState = {
  hasError: false,
  error: null,
};

class ErrorBoundary extends Component<ErrorBoudaryProps, ErrorBoundaryState> {
  constructor(props: ErrorBoudaryProps) {
    super(props);
    this.state = initialState;
  }

  static getDerivedStateFromError(error: Error) {
    // Update state so the next render will show the fallback UI.
    return { hasError: true, error };
  }

  componentDidCatch(error: Error, info: ErrorInfo) {
    logError(error, info.componentStack);
  }

  render() {
    if (this.state.hasError && this.props.fallbackComponent) {
      // Render any custom fallback UI
      return createElement(this.props.fallbackComponent, {
        error: this.state.error,
      });
    }

    return this.props.children;
  }
}

function logError(error: Error, componentStack?: string | null) {
  // Example "componentStack":
  //   in ComponentThatThrows (created by App)
  //   in ErrorBoundary (created by App)
  //   in div (created by App)
  //   in App
  console.error("Oops! Error:", error.message, componentStack);
}

export default ErrorBoundary;
