import React, { useState } from 'react';
import * as R from 'ramda';
import { connect } from 'react-redux';

import { filter, resetFilter } from '../store/actions';

const Filter = ({ config, filter, resetFilter }) => {
  const [state, setstate] = useState([]);
  const handleSelect = item => {
    filter(item);
    if (state.includes(item)) {
      setstate(state.filter(i => i !== item));
    } else {
      setstate([...state, item]);
    }
  };
  return (
    <>
      <ul>
        {R.keys(config).map(cur => (
          <li key={cur} onClick={() => handleSelect(cur)}>
            {`${config[cur][0].code}/${config[cur][1].code}`}{' '}
          </li>
        ))}
        <li onClick={() => resetFilter()}>clear selection</li>
      </ul>
    </>
  );
};

export default connect(
  null,
  {
    filter,
    resetFilter
  }
)(Filter);
