export const FETCH_CONFIGURATION = 'RATES/FETCH_CONFIGURATION';
export const FETCH_RATES = 'RATES/FETCH_RATES';
export const SET_RATES = 'RATES/SET_RATES';

export const setRates = rates => ({
  type: SET_RATES,
  payload: rates,
});

export const fetchConfiguration = () => {
  return (dispatch, _, { fetchJSON, toast }) => {
    dispatch({
      type: FETCH_CONFIGURATION,
      payload: fetchJSON({
        url: '/configuration',
      }).catch(e => {
        toast.error(e.data.message);
        throw e;
      }),
    });
  };
};

export const fetchRates = currencyPairs => {
  return (dispatch, _, { fetchJSON, toast }) => {
    dispatch({
      type: FETCH_RATES,
      meta: {
        fetchRatesId: Math.random(),
      },
      payload: fetchJSON({
        url: `/rates?currencyPairIds[]=${currencyPairs.join(
          '&currencyPairIds[]=',
        )}
        `,
      }).catch(e => {
        toast.warn('Failed to update data. Displayed data can be outdated.');
        throw e;
      }),
    });
  };
};
