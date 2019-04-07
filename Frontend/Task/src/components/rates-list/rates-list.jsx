import React from 'react';
import PropTypes from 'prop-types';

import RatesListItem from './rates-list-item.jsx';

import styles from './rates-list.module.scss';

/**
 * Renders currencies rates list
 */
const RatesList = ({ items }) => (
  <ul className={styles.ratesList}>
    {items.map(item => (
      <RatesListItem key={item.value} {...item} />
    ))}
  </ul>
);

RatesList.defaultProps = {
  items: [],
};

RatesList.propTypes = {
  items: PropTypes.array,
};

export default RatesList;
