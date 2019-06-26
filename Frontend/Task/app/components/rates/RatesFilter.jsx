import React from 'react';
import PropTypes from 'prop-types';

import './RatesFilter.scss';

const RatesFilter = (props) => {
  const handleFilter = (e) => props.onFilter(e.currentTarget.value);

  return (
    <div className="RatesFilter">
      <label>
        <span className="RatesFilter__text">Filter rate:</span>
        <input type="text" onChange={handleFilter} className="RatesFilter__input" />
      </label>
    </div>
  );
};


RatesFilter.propTypes = {
  onFilter: PropTypes.func
}

export default RatesFilter;