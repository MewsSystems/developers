import { Component, ReactNode } from "react";

import { Link, Typography } from "@mui/material";

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
          <Typography variant="h1">Ooooops</Typography>
          <Typography variant="body1" sx={{ mt: 2, mb: 5 }}>
            We're sorry, but it seems like there's been an error. Please try
            going back to the homepage and navigating from there.
          </Typography>

          <Link href="/">Go back to homepage</Link>
        </>
      );

    return children;
  }
}
