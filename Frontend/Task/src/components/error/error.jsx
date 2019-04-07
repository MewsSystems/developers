import React from 'react';
import styles from './error.module.scss';

/**
 * Display error message placeholder
 */
const Error = () => (
  <div className={styles.errorContainer}>
    <i className='fas fa-exclamation-triangle' />
    <h1>
      There was an error while loading application data.
      <br />
      Please, try again later
    </h1>
  </div>
);

export default Error;
