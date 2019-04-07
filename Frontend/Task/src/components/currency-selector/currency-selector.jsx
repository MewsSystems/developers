import React from 'react';
import PropTypes from 'prop-types';
import Select from 'react-select';

import { getOptions } from '../../helpers/common';

/**
 * Render select component to choose currencies
 */
const CurrencySelector = ({
  currencies,
  isDisabled,
  value,
  handleChange,
}) => {
  const optionsStyles = {
    option: (provided, state) => ({
      ...provided,
      borderBottom: '1px dotted pink',
      color: state.isSelected ? 'red' : 'blue',
      padding: 12,
    }),
    control: () => ({
      display: 'flex',
      backgroundColor: 'white',
      border: '2px solid black',
      width: 400,
    }),
  };

  return (
    <Select
      styles={optionsStyles}
      isMulti
      isDisabled={isDisabled}
      value={value}
      onChange={handleChange}
      options={getOptions(currencies)}
    />
  );
};

CurrencySelector.defaultProps = {
  currencies: {},
  isDisabled: false,
  value: null,
};

CurrencySelector.propTypes = {
  currencies: PropTypes.object,
  isDisabled: PropTypes.bool,
  handleChange: PropTypes.func.isRequired,
  value: PropTypes.array,
};

export default CurrencySelector;
