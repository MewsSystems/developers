import React, { ChangeEvent, FC, useState } from 'react';
import { Wrapper } from './styles';
import { setQueryParams, triggerSearch } from '../../actions/search';
import { DebouncedInput } from './DebouncedInput';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';
import { Checkbox } from './Checkbox';

const SearchComponent: FC<{ query?: string; queryParams?: any; dispatch?: Dispatch }> = ({
  query,
  queryParams,
  dispatch,
}) => {
  const options = [{ label: 'Include Adult', value: 'include_adult' }];

  const handleCheckboxChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { value, checked } = e.target;
    dispatch(setQueryParams({ [value]: checked }));
  };

  return (
    <Wrapper>
      <DebouncedInput
        handleOnChange={triggerSearch}
        placeholder={'Search for a film...'}
        defaultValue={query}
      />
      {options.map((option) => (
        <Checkbox
          key={option.value}
          label={option.label}
          value={option.value}
          isChecked={!!queryParams?.[option.value]}
          onChange={handleCheckboxChange}
        />
      ))}
    </Wrapper>
  );
};

const mapStateToProps = (state: any) => {
  return state.search;
};

export default connect(mapStateToProps)(SearchComponent);
