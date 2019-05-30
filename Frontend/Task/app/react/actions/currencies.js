import constants from '../constants';

export const fetchConfig = () => ({
  type: constants.FETCH_CONFIG,
  payload: {
    promise: fetch(`/configuration`, {
      method: 'GET'
    })
    .then(response => {
      if (response.status === 200) {
        return response.json();
      }
    })
    .catch(response => response.error)
  }
});
