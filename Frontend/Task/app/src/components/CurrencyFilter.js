import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import { setFilterValue } from '../actions';


const CurrencyFilter = (props) => {
  const { filteredValue } = props;

  useEffect(() => {
    props.setFilterValue(filteredValue);
  });

  const changeValue = (val) => {
    props.setFilterValue(val);
  };


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
              onChange={e => changeValue(e.target.value)}
              value={filteredValue}
            />
          </div>
        </div>
      </form>
    </div>
  );
};

CurrencyFilter.propTypes = {
  setFilterValue: PropTypes.func.isRequired,
  filteredValue: PropTypes.string,
};
CurrencyFilter.defaultProps = {
  filteredValue: '',
};
const mapStateToProps = state => ({ filteredValue: state.currencies.filteredValue });

export default connect(mapStateToProps, { setFilterValue })(CurrencyFilter);
