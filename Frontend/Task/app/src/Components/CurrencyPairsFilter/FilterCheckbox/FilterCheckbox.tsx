import React from 'react';
import IFilterCheckbox from '../../../interfaces/FilterCheckbox.interface'

const FilterCheckbox = (props: IFilterCheckbox) => {
  const { id, filter, currencyPair, toggleFilter } = props;
  const isChecked = !(filter.filter((pairsID: string) => pairsID === id).length > 0);
  return (
    <label>
      <input type="checkbox" name="checkbox" value={id} checked={isChecked} onChange={() => toggleFilter(id, isChecked)} />
      {currencyPair[0].code}/{currencyPair[1].code}
    </label>
  );
}

export default FilterCheckbox;

