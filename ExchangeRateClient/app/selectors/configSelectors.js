import { createSelector } from 'reselect';
import { prop } from 'ramda';

export const getConfig = prop('config');

export const getEndpoint = createSelector([getConfig], prop('endpoint'));

export const getInterval = createSelector([getConfig], prop('interval'));
