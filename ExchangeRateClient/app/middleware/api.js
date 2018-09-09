import axios from 'axios';
import { endpoint } from '../config';
// import { GET_CURRENCY_PAIRS_LOADING } from '../actions/types';
import {
  getCurrencyPairsLoading,
  getCurrencyPairsSuccess,
  getCurrencyPairsError,
  getRatesLoading,
  getRatesSuccess,
  getRatesError
} from '../actions/currencyPairsAction';

export const fetchPairs = () => dispatch => {
  dispatch(getCurrencyPairsLoading());

  return axios({
    url: `${endpoint}/configuration`,
    method: 'get'
  })
    .then(res => {
      dispatch(getCurrencyPairsSuccess(res.data));
      return res;
    })
    .catch(error => {
      dispatch(getCurrencyPairsError(error));
      return error;
    });
};

// http://localhost:3000/rates?currencyPairIds=5b428ac9-ec57-513d-8a08-20199469fb4d&currencyPairIds=5b98842f-bfe5-5564-b321-068763d7e2a3

// export const updateRate = () => dispatch => {
//   // dispatch(getRatesLoading());

//   return axios({
//     url: `${endpoint}/rates`,
//     method: 'request',
//     params: {
//       currencyPairIds: Object.keys(this.props.currencyRates)
//     }
//   })
//     .then(res => {
//       dispatch(getRatesSuccess(res.data));
//       return res;
//     })
//     .catch(error => {
//       dispatch(getRatesError(error));
//       return error;
//     });
// };
// setInterval(updateRate, 5000);
