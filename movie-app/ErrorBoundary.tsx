import { Component, ReactNode } from "react";

type Props = {
  children?: ReactNode;
};

type State = {
  error?: unknown;
};

export class ErrorBoundary extends Component<Props, State> {
  constructor(props: Props) {
    super(props);

    this.state = {
      error: undefined,
    };
  }

  static getDerivedStateFromError(error: unknown) {
    return { error };
  }

  render() {
    const { children } = this.props;
    const { error } = this.state;

    if (error)
      return (
        <>
          <div>
            <h1>Ooooops</h1>
            <p>
              We're sorry, but it seems like there's been an error. Please try
              going back to the homepage and navigating from there.
            </p>

            <a href={"/"}>Go back to homepage</a>
          </div>
        </>
      );

    return children;
  }
}
