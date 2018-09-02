import { createSelector } from 'reselect';
import { prop } from 'ramda';

export const getFetchingStates = prop('isFetching');

export const isFetching = (dataName) => createSelector([getFetchingStates], prop(dataName));
