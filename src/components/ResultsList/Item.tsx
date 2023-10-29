import React, { FC, SyntheticEvent } from 'react';

import { Movie, Person } from '../../types';
import { SearchOptionType } from '../../reducers/searchReducer';
import { MovieItem } from './Movie';
import { PersonItem } from './Person';

export type ItemType = FC<{
  result: Movie & Person;
  onNavigation?: (result: Movie) => void;
  toggleInfo?: (id?: number) => void;
  isSelected?: boolean;
  itemType?: SearchOptionType;
}>;

export const Item: ItemType = (props) => {
  if (props.itemType === 'person') return <PersonItem {...props} />;
  return <MovieItem {...props} />;
};
