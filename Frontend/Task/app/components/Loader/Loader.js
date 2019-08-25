import React from 'react';
import styles from 'Components/Loader/Loader.module.css';

const Loader = () => (
  <div className={styles.centered}>
    <div className={styles.blob_left}></div>
    <div className={styles.blob_right}></div>
  </div>
)

export default Loader;
