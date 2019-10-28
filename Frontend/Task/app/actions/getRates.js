import axios from 'axios'
import { toast } from 'react-toastify'

import { actionTypes, RATES_URL } from '../constants'

const requestRates = () => ({
  type: actionTypes.RATE_LIST_FETCHING,
})

const receiveRates = rates => ({
  type: actionTypes.RATE_LIST_DONE,
  rates,
})

const fetchFailed = message => ({
  type: actionTypes.RATE_LIST_FAILED,
  message,
})

export const getRates = dispatch => currencyPairIds => {
  dispatch(requestRates())

  const queryString = `currencyPairIds=${currencyPairIds.join(
    '&currencyPairIds='
  )}`

  const rateRequest = axios.get(`${RATES_URL}/?${queryString}`)

  rateRequest
    .then(({ data: { rates } }) => dispatch(receiveRates(rates)))
    .catch(err => {
      dispatch(fetchFailed(err))
      toast('Fetching failed, please wait for the next update')
    })
}
