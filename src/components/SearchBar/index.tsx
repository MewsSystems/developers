import React, { FC } from 'react';
import { HeaderText } from '../HeaderText';
import { Wrapper } from './styles';
import { triggerSearch } from '../../actions/search';
import { DebouncedInput } from './DebouncedInput';

export const SearchBar: FC = () => {
  return (
    <Wrapper>
      <HeaderText>Search Movies Here:</HeaderText>
      <DebouncedInput handleOnChange={triggerSearch} placeholder={'Search for a film...'} />
    </Wrapper>
  );
};
