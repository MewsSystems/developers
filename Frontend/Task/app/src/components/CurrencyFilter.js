import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import { setFilterValue } from '../actions';


const CurrencyFilter = (props) => {
  const [value, setValue] = useState('');

  useEffect(() => {
    props.setFilterValue(value);
  });

  return (
    <div className="container">
      <form>
        <div>
          <label htmlFor="filterInput">Filter currency</label>
          <input
            id="filterInput"
            name="filteredValue"
            onChange={e => setValue(e.target.value)}
            value={value}
          />
        </div>
      </form>
    </div>
  );
};

CurrencyFilter.propTypes = {
  setFilterValue: PropTypes.func.isRequired,
};

export default connect(null, { setFilterValue })(CurrencyFilter);
