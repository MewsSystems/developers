import Axios from 'axios';

export const searchText = text => ({
  type: 'SEARCH_TEXT',
  payload: text,
});

export const getData = () => (dispatch, getState) => {
  const { configuration, data } = getState();
  let oldData;
  if (Object.keys(data).length !== 0) {
    oldData = data;

    dispatch({
      type: 'DATA',
      payload: {},
    });
  }

  Object.keys(configuration).forEach(key => {
    const request = {
      params: {
        currencyPairIds: [key],
      },
    };

    Axios.get('http://localhost:3000/rates', request, {
      timeout: 12000,
    })
      .then(response => {
        const rate = parseFloat(Object.values(response.data.rates));
        let trend = 'N/A';

        if (oldData) {
          trend =
            oldData[key].rate < rate
              ? 'UP'
              : oldData[key].rate > rate
              ? 'DOWN'
              : oldData[key].rate === rate
              ? 'EQUAL'
              : 'N/A';
        }
        data[key] = {
          rate,
          status: response.status,
          statusText: response.statusText,
          trend,
        };

        dispatch({
          type: 'DATA',
          payload: data,
        });
      })
      .catch(error => {
        data[key] = {
          rate: 'N/A',
          status: error.response.status,
          statusText: error.response.statusText,
          trend: 'N/A',
        };
        dispatch({
          type: 'DATA',
          payload: data,
        });
      });
  });
};

export const getConfiguration = () => dispatch => {
  Axios.get('http://localhost:3000/configuration', { timeout: 10000 })
    .then(response => {
      const configuration = response.data.currencyPairs;
      dispatch({
        type: 'CONFIGURATION',
        payload: configuration,
      });
      dispatch(getData());
    })
    .catch(error => console.log('getConfiguration error', error));
};
