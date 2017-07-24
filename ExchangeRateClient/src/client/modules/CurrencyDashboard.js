import React, {Component} from 'react'
import {interval as configReloadInterval} from 'config/config'

import {getConfiguration, getRates} from 'client/api/exchangeRates'
import CurrencyDashboardView from 'client/components/CurrencyDashboard/CurrencyDashboardView'

export default class CurrencyDashboard extends Component {
  _getRatesInterval = null

  componentDidMount () {
    this.apiGetConfiguration()
  }

  componentWillUnmount () {
    clearInterval(this._getRatesInterval)
  }

  state = {
    isLoading: true,
    error: false,
    errorMessage: '',
    ids: [],
    data: {},
    rates: null,
  }

  apiGetConfiguration = () => {
    getConfiguration()
      .then((response) => {
        this.apiGetConfigurationSuccess(response)
      })
      .catch((error) => {
        this.apiGetConfigurationFail(error)
      })
  }

  apiGetRates = (ids) => {
    const currencyPairIds = ids || this.state.ids

    getRates(currencyPairIds)
      .then((response) => {
        this.apiGetRatesSuccess(response)
      })
      .catch((error) => {
        this.apiGetRatesFail(error)
      })
  }

  apiGetConfigurationSuccess = ({currencyPairs}) => {
    const ids = Object.keys(currencyPairs)

    this.setState({
      data: currencyPairs,
      ids,
    })

    this.apiGetRates()
    this._getRatesInterval = setInterval(this.apiGetRates, configReloadInterval)
  }

  apiGetConfigurationFail = (errorMessage) => {
    this.setState({
      error: true,
      errorMessage: errorMessage.message,
      isLoading: false,
      data: {},
    })
  }

  apiGetRatesSuccess = (response) => {
    const {rates = {}} = this.state
    const returnedRates = response.rates
    const newRates = {}

    Object.keys(returnedRates).forEach((rate) => {
      newRates[rate] = {
        value: returnedRates[rate],
        previousValue: rates && rates[rate] != null && rates[rate].value, // eslint-disable-line eqeqeq
      }
    })

    this.setState({
      rates: newRates,
      isLoading: false,
    })
  }

  apiGetRatesFail = (errorMessage) => {
    this.setState({
      error: true,
      errorMessage: errorMessage.message,
      isLoading: false,
    })
  }

  render () {
    return (
      <CurrencyDashboardView
          {...this.state}
      />
    )
  }
}
