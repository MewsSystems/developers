import React from 'react';
import PropTypes from 'prop-types';
import classnames from 'classnames';

import './Rate.scss';

const Rate = (props) => {
  const trendClassName = classnames('Rate__trend', {
    'Rate__trend--stagnating': props.trend === 'stagnating',
    'Rate__trend--growing': props.trend === 'growing',
    'Rate__trend--declining': props.trend === 'declining',
  });

  return (
    <div className="Rate">
      <label className="Rate__displayName">{props.displayName}</label>
      <div className="Rate__currentRate">{props.currentRate || '0.0'}</div>
      <div className={trendClassName}>{props.trend || '~'}</div>
    </div>
  );
};

Rate.propTypes = {
  currentRate: PropTypes.number,
  displayName: PropTypes.string,
  trend: PropTypes.string
}

export default Rate;
