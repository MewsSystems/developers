import React from 'react';
import * as R from 'ramda';
import { connect } from 'react-redux';

import { filterCurrencies, resetFilter } from '../store/actions';
import { FilterWrapper, FilterItem, FilterReset } from '../assets/Styles';
import Spinner from '../assets/UI/Spinner';

const Filter = ({ config, filterCurrencies, resetFilter, filtered }) => {
  const handleSelect = item => {
    filterCurrencies(item);
  };
  const renderFilter = () => {
    if (!R.isEmpty(config)) {
      return (
        <FilterWrapper>
          {R.keys(config).map(cur => (
            <FilterItem
              key={cur}
              onClick={() => handleSelect(cur)}
              active={filtered.includes(cur)}
            >
              {`${config[cur][0].code}/${config[cur][1].code}`}{' '}
            </FilterItem>
          ))}
          <FilterReset onClick={() => resetFilter()}>reset</FilterReset>
        </FilterWrapper>
      );
    }
    return <Spinner />;
  };
  return renderFilter();
};

const mapStateToProps = state => {
  return {
    filtered: state.filtered
  };
};

export default connect(
  mapStateToProps,
  {
    filterCurrencies,
    resetFilter
  }
)(Filter);
