import React from 'react';
import PropTypes from 'prop-types';
import {connect} from 'react-redux';
import sn from 'classnames';
import s from '../styles/Rates.less';
import {rateTrend} from '../const';
import {getFilteredRates} from '../selectors';


const Rates = ({items}) => (
  <div className={s.rates}>
    {items.map((item, i) => (
      <div key={item.id} className={s.item}>
        <div className={s.title}>{item.name}</div>
        <div className={s.value}>
          {item.value || '—'}
          <div
            className={sn(s.trend, {
              [s.trendGrow]: item.trend === rateTrend.growing,
              [s.trendDecl]: item.trend === rateTrend.declining
            })}
          />
        </div>
      </div>
    ))}
  </div>
);

Rates.propTypes = {
  items: PropTypes.arrayOf(PropTypes.shape({
    id: PropTypes.string,
    name: PropTypes.string,
    value: PropTypes.number,
    trend: PropTypes.string,
  }))
};

Rates.defaultProps = {
  items: []
};

export default connect(state => ({
  items: getFilteredRates(state)
}))(Rates);
