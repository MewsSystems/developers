import { toast } from 'react-toastify';

export const FETCH_CONFIGURATION = 'RATES/FETCH_CONFIGURATION';
export const FETCH_RATES = 'RATES/FETCH_RATES';
export const SET_RATES = 'RATES/SET_RATES';

export const setRates = rates => ({
  type: SET_RATES,
  payload: rates,
});

export const fetchConfiguration = () => {
  return (dispatch, _, { fetchJSON, getConfig }) => {
    dispatch({
      type: FETCH_CONFIGURATION,
      payload: fetchJSON({
        url: '/configuration',
      })
        .catch(e => {
          toast.error(e.data.message);
          throw e;
        })
        .then(response => {
          return response;
        }),
    });
  };
};

export const fetchRates = currencyPairs => {
  return (dispatch, _, { fetchJSON, getConfig }) => {
    dispatch({
      type: FETCH_RATES,
      payload: fetchJSON({
        url: `/rates?currencyPairIds[]=${currencyPairs.join(
          '&currencyPairIds[]=',
        )}
        `,
      })
        .catch(e => {
          toast.warn('Failed to update data. Displayed data can be outdated.');
          throw e;
        })
        .then(response => {
          return response;
        }),
    });
  };
};
