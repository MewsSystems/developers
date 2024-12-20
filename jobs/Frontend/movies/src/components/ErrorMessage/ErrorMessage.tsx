import React from 'react';
import styles from './ErrorMessage.module.css';

interface ErrorMessageProps {
  children: string;
}

const ErrorMessage: React.FC<ErrorMessageProps> = ({ children }) => {
  return (
    <div className={styles.errorContainer}>
      <p>{children}</p>
    </div>
  );
};

export default ErrorMessage;
