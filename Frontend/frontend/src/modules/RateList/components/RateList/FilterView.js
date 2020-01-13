import React from 'react';
import {
  func, shape, string, object,
} from 'prop-types';

import { RATES_TABLE_FILTERS, } from '../../../../globals';
import SortableTH from '../../../../components/Table/SortableTH';
import Input from '../../../../atoms/Input/Input';


const FilterView = ({
  filter: {
    values: {
      name,
    },
    sort,
  },
  onChangeSort,
  onChangeValue,
}) => (
  <thead>
    <tr>
      <SortableTH
        id={RATES_TABLE_FILTERS.NAME}
        label="Name"
        value={sort}
        onChangeSort={onChangeSort}
        className="rateList--table-th1"
      />
      <SortableTH
        id={RATES_TABLE_FILTERS.RATE}
        label="Rate"
        value={sort}
        onChangeSort={onChangeSort}
        className="rateList--table-th2"
      />
      <SortableTH
        id={RATES_TABLE_FILTERS.TREND}
        label="Trend"
        value={sort}
        onChangeSort={onChangeSort}
        className="rateList--table-th3"
      />
    </tr>
    <tr>
      <th>
        <Input
          value={name}
          onChange={(e) => onChangeValue('name', e.target.value)}
          size="sm"
        />
      </th>
      <th />
      <th />
    </tr>
  </thead>
);


FilterView.propTypes = {
  filter: shape({
    values: shape({
      name: string.isRequired,
    }).isRequired,
    sort: object,
  }).isRequired,
  onChangeSort: func.isRequired,
  onChangeValue: func.isRequired,
};


export default FilterView;
