import React, { ChangeEvent, FC } from 'react';
import { Wrapper, OptionsSection, Radio, RadioLabel } from './styles';
import { setQueryParams, triggerSearch, triggerSearchOnChange } from '../../actions/search';
import { DebouncedInput } from './DebouncedInput';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';
import { Checkbox } from './Checkbox';
import { SearchOptionType } from '../../reducers/searchReducer';

const options = [{ label: 'Include Adult', value: 'include_adult' }];

const searchOptions = [
  { id: 1, label: 'All', value: 'multi' },
  { id: 2, label: 'Movie', value: 'movie' },
  { id: 3, label: 'TV', value: 'tv' },
  { id: 4, label: 'Person', value: 'person' },
  { id: 5, label: 'Company', value: 'company' },
];

const SearchComponent: FC<{
  query?: string;
  queryParams?: any;
  searchType: SearchOptionType;
  dispatch: Dispatch;
}> = ({ query, queryParams, searchType, dispatch }) => {
  const handleCheckboxChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { value, checked } = e.target;
    dispatch(setQueryParams({ [value]: checked }));
  };

  const handleOptionChange = (e: ChangeEvent<HTMLInputElement>) => {
    const selectedValue = e.target.value;
    dispatch(triggerSearchOnChange(selectedValue));
  };

  return (
    <Wrapper>
      <DebouncedInput
        handleOnChange={(value) => {
          dispatch(triggerSearch(value));
        }}
        placeholder={'Search for a film...'}
        defaultValue={query}
      />

      <OptionsSection>
        {searchOptions.map((option) => (
          <RadioLabel key={option.id}>
            <Radio
              value={option.value}
              defaultChecked={option.id === 1}
              checked={option.value === searchType}
              onChange={handleOptionChange}
            />
            {option.label}
          </RadioLabel>
        ))}

        {options.map((option) => (
          <Checkbox
            key={option.value}
            label={option.label}
            value={option.value}
            isChecked={!!queryParams?.[option.value]}
            onChange={handleCheckboxChange}
          />
        ))}
      </OptionsSection>
    </Wrapper>
  );
};

const mapStateToProps = (state: any) => {
  return state.search;
};

export default connect(mapStateToProps)(SearchComponent);
