import React, { Component } from 'react';
import PropTypes from 'prop-types';

import styles from './ErrorBoundary.module.css';

// The ErrorBoundary component contains the effects of any errors that have not already been
// allowed for at an appropriate level and presents the user with a warning message
class ErrorBoundary extends Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false };
  }

  static getDerivedStateFromError() {
    return { hasError: true }; // next render will respond to error
  }

  componentDidCatch(error, errorInfo) {
    /* eslint no-console: 0 */
    console.log(`Error caught: ${error},\n${JSON.stringify(errorInfo, null, 2)}`);
  }

  render() {
    const { children } = this.props;
    if (!children) return null; // nothing currently rendered within this error boundary

    const { hasError } = this.state;
    if (hasError) {
      return (
        <div className={styles.wrapper}>
          <h2 className={styles.title}>Error</h2>
          <p className={styles.message}>
            Sorry, something has gone wrong with this part of the application. Please
            try refreshing the page in case it is a temporary problem.
          </p>
        </div>
      );
    }
    return children;
  }
}

ErrorBoundary.propTypes = {
  children: PropTypes.node,
};
ErrorBoundary.defaultProps = {
  children: null,
};

export default ErrorBoundary;
