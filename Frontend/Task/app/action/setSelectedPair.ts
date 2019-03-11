import { Dispatch } from 'react';
import { ExchangeAction } from '../interface/exchangeActionInterface';

export default (pairId: string) => (dispatch: Dispatch<any>) => {
  localStorage.setItem('pairId', pairId);
  dispatch({type: ExchangeAction.SET_SELECTED, payload: pairId});
};
