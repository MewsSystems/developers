import React from 'react';
import Select from 'react-select';

const CurrencySelect = ({ handleChange, options, value }) => {
  return (
    <Select isMulti value={value} onChange={handleChange} options={options} />
  );
};

export default CurrencySelect;
