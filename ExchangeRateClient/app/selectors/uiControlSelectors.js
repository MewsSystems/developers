import { createSelector } from 'reselect';
import { prop } from 'ramda';

export const getUiControl = prop('uiControl');

export const getShowCountdown = createSelector([getUiControl], prop('showCountdown'));

export const getTables = createSelector([getUiControl], prop('tables'));

export const getTable = (tableName) => createSelector([getTables], prop(tableName));

export const getCurrentPage = (tableName) => createSelector([getTable(tableName)], prop('currentPage'));
