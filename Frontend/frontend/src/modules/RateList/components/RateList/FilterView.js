import React from 'react';
import {
  func, shape, string, object,
} from 'prop-types';

import SortableTH from '../../../../components/Table/SortableTH';


const FilterView = ({
  filter: {
    currencyL,
    currencyR,
    sort,
  },
  onChangeSort,
  onChangeValue,
}) => (
  <>
    <tr>
      <SortableTH
        id="from"
        label="From"
        value={sort}
        onChangeSort={onChangeSort}
      />
      <SortableTH
        id="to"
        label="To"
        value={sort}
        onChangeSort={onChangeSort}
      />
      <SortableTH
        id="rate"
        label="Rate"
        value={sort}
        onChangeSort={onChangeSort}
      />
      <SortableTH
        id="trend"
        label="Trend"
        value={sort}
        onChangeSort={onChangeSort}
      />
    </tr>
    <tr>
      <th>
        <input
          value={currencyL}
          onChange={(e) => onChangeValue('currencyL', e.target.value)}
        />
      </th>
      <th>
        <input
          value={currencyR}
          onChange={(e) => onChangeValue('currencyR', e.target.value)}
        />
      </th>
      <th />
      <th />
    </tr>
  </>
);


FilterView.propTypes = {
  filter: shape({
    currencyL: string.isRequired,
    currencyR: string.isRequired,
    sort: object,
  }).isRequired,
  onChangeSort: func.isRequired,
  onChangeValue: func.isRequired,
};


export default FilterView;
