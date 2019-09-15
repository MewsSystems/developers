import { createSelector } from 'reselect'
import { ApplicationState } from '../store/types';

export const getIdsList = createSelector(
    (state: ApplicationState) => state.currencyPairs,
    (currencyPairs) => {

      return  currencyPairs && Object.keys(currencyPairs);
    }
  )