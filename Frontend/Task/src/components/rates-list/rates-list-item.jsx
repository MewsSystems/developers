import React from 'react';
import PropTypes from 'prop-types';

import styles from './rates-list.module.scss';

/**
 * Define and render trend for currency item
 */
const getCurrencyTrend = (prev, next) => {
  if (!prev || !next) {
    return <i title='Unknown' className="far fa-question-circle grey" />;
  }
  if (prev < next) {
    return <i title='Growing' className="fas fa-arrow-alt-circle-up green" />;
  }
  if (prev > next) {
    return <i title='Declining' className="fas fa-arrow-alt-circle-down red" />;
  }
  if (prev === next) {
    return <i title='Stagnating' className="fas fa-equals grey" />;
  }
  return <i title='Unknown' className="far fa-question-circle grey" />;
};

/**
 * Renders rates list item
 */
const RatesListItem = ({
  label,
  next,
  prev,
}) => (
  <li className={styles.ratesListItemWrapper}>
    <span>{label}</span>
    <span>{next}</span>
    <span>{getCurrencyTrend(prev, next)}</span>
  </li>
);

RatesListItem.defaultProps = {
  next: null,
  prev: null,
};

RatesListItem.propTypes = {
  label: PropTypes.string.isRequired,
  next: PropTypes.number,
  prev: PropTypes.number,
};

export { RatesListItem };
export default React.memo(RatesListItem);
