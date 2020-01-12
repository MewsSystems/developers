import React from 'react';
import {
  func, shape, string, object,
} from 'prop-types';

import { RATES_TABLE_FILTERS, } from '../../../../globals';
import SortableTH from '../../../../components/Table/SortableTH';


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
  <>
    <tr>
      <SortableTH
        id={RATES_TABLE_FILTERS.NAME}
        label="Name"
        value={sort}
        onChangeSort={onChangeSort}
      />
      <SortableTH
        id={RATES_TABLE_FILTERS.RATE}
        label="Rate"
        value={sort}
        onChangeSort={onChangeSort}
      />
      <SortableTH
        id={RATES_TABLE_FILTERS.TREND}
        label="Trend"
        value={sort}
        onChangeSort={onChangeSort}
      />
    </tr>
    <tr>
      <th>
        <input
          value={name}
          onChange={(e) => onChangeValue('name', e.target.value)}
        />
      </th>
      <th />
      <th />
    </tr>
  </>
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
