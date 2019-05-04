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
    <div className="p-3">
      <form>
        <div>
          <div className="input-group input-group-sm">
            <div className="input-group-prepend">
              <span className="input-group-text" id="inputGroup-sizing-sm">Filter currency:</span>
            </div>
            <input
              type="text"
              id="filterInput"
              name="filteredValue"
              className="form-control"
              aria-label="Sizing example input"
              aria-describedby="inputGroup-sizing-sm"
              onChange={e => setValue(e.target.value)}
              value={value}
            />
          </div>
        </div>
      </form>
    </div>
  );
};

CurrencyFilter.propTypes = {
  setFilterValue: PropTypes.func.isRequired,
};

export default connect(null, { setFilterValue })(CurrencyFilter);
