import axios from 'axios';
import {
  GET_CONFIG,
  GET_RATES,
  SPINNER_LOADING,
} from './types';

// on Change phone 
// export const phoneChanged = (text) => {
//   return {
//     type: PHONE_CHANGED,
//     payload: text
//   };
// };

// Get Configuration
export const getConfiguration = () => dispatch => {
  axios
  .get(`http://localhost:3000/configuration`)
  .then(res =>
      dispatch({
        type: GET_CONFIG,
        payload: res.data.currencyPairs
      })
    )
    .catch(err =>console.log(err)
    );
};
// Get currency Rates
export const getRates = (numbers) => dispatch => {
  axios
  .get(`http://localhost:3000/rates?currencyPairIds=${numbers}`)
  .then(res =>
    dispatch({
      type: GET_RATES,
      payload: res.data.rates,
      rateKey:numbers
    })
    )
    .catch(err =>
      dispatch(getRates(numbers))
    );
};








