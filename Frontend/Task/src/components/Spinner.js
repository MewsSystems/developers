import React from 'react';
import logo from '../assets/logo.svg';
import styles from './Spinner.module.css';

const Spinner = () => (
  <img src={logo} className={styles.spinner} alt="spinner" />
);

export default Spinner;
