// @flow strict

import * as React from 'react';
import styled from 'styled-components';

import { COLORS } from '../utils/constants';
import Text from './Text';

type Props = {|
  +children: React.Node,
|};
type State = {|
  hasError: boolean,
  message: string,
|};

const Alert = styled.div`
  background-color: ${COLORS.CRITICAL};
  padding: 24px 8px;
`;

class ErrorBoundary extends React.Component<Props, State> {
  static getDerivedStateFromError(error: Error) {
    // Update state so the next render will show the fallback UI.
    return { hasError: true, message: error.message };
  }

  state = {
    hasError: false,
    message: '',
  };

  componentDidCatch(error: Error) {
    // eslint-disable-next-line no-console
    console.error(error);
  }

  render() {
    return this.state.hasError ? (
      <Alert>
        <Text color={COLORS.BAKCGROUND}>Unexpected error occured: {this.state.message}</Text>
      </Alert>
    ) : (
      this.props.children
    );
  }
}

export function withErrorBoundary<T>(Component: React.ComponentType<T>) {
  return (props: T) => (
    <ErrorBoundary>
      <Component {...props} />
    </ErrorBoundary>
  );
}

export default ErrorBoundary;
