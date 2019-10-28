import axios from 'axios'

import { actionTypes, CONFIG_URL } from '../constants'

const requestPairs = () => ({
  type: actionTypes.CURRENCY_LIST_FETCHING,
})

const receivePairs = currencyPairs => ({
  type: actionTypes.CURRENCY_LIST_DONE,
  currencyPairs,
})

const fetchFailed = message => ({
  type: actionTypes.CURRENCY_LIST_FAILED,
  message,
})

export const getPairs = dispatch => () => {
  dispatch(requestPairs())

  const configRequest = axios.get(`${CONFIG_URL}`)

  configRequest
    .then(({ data: { currencyPairs } }) =>
      dispatch(receivePairs(currencyPairs))
    )
    .catch(err => dispatch(fetchFailed(err)))
}
