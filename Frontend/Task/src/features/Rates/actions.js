import { toast } from 'react-toastify';

export const FETCH_CONFIGURATION = 'RATES/FETCH_CONFIGURATION';
export const FETCH_RATES = 'FETCH_RATES';
export const SET_RATES = 'SET_RATES';

export const setRates = rates => ({
  type: SET_RATES,
  payload: rates,
});

function handleStatus(response) {
  if (!response.ok) {
    throw new Error(response.statusText);
  }
  return response;
}

export const fetchConfiguration = () => {
  return (dispatch, _, { getConfig }) => {
    dispatch({
      type: FETCH_CONFIGURATION,
      payload: fetch(`${getConfig().endpoint}/configuration`)
        .then(handleStatus)
        .then(response => response.json())
        .catch(e => {
          toast.error(e.message);
          throw e;
        }),
    });
  };
};

export const fetchRates = currencyPairs => {
  return (dispatch, _, { getConfig }) => {
    const toastId = 'fetchRates';
    dispatch({
      type: FETCH_RATES,
      payload: fetch(
        `${getConfig().endpoint}/rates?currencyPairIds[]=${currencyPairs.join(
          '&currencyPairIds[]=',
        )}
        `,
      )
        .then(handleStatus)
        .then(response => {
          toast.dismiss(toastId);
          return response.json();
        })
        .catch(e => {
          toast.warn('Failed to update data. Displayed data can be outdated.', {
            toastId: toastId,
          });
          throw e;
        }),
    });
  };
};
