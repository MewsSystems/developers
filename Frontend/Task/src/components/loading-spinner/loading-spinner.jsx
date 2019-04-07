import React from 'react';
import { Transition } from 'react-transition-group';
import styles from './loading-spinner.module.scss';

/**
 * Draw loading indicator
 */
const LoadingSpinner = () => {
  const defaultStyle = {
    transition: 'all 100ms ease-out',
    opacity: 0,
  };

  const transitionStyles = {
    entering: { opacity: 0 },
    entered: { opacity: 0.3 },
  };

  return (
    <Transition in appear timeout={0}>
      {state => (
        <div
          className={styles['data-loading-spinner__container']}
          style={{
            ...defaultStyle,
            ...transitionStyles[state],
          }}
        >
          <div className={styles['looping-rhombuses-spinner']}>
            <div className={styles.rhombus} />
            <div className={styles.rhombus} />
            <div className={styles.rhombus} />
          </div>
        </div>
      )}
    </Transition>
  );
};

export default React.memo(LoadingSpinner);
