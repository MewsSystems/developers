import React, {Component} from 'react'
import {interval as configReloadInterval} from 'config/config'
import {connect} from 'react-redux'

import {getConfiguration, getRates} from 'client/api/exchangeRates'
import {toggleFilter, updateFilterValue} from 'client/actions/exchangeRatesActions'
import CurrencyDashboardView from 'client/components/CurrencyDashboard/CurrencyDashboardView'

const mapStateToProps = (state) => ({
  filterValue: state.exchangeRatesReducer.filterValue,
  isFilterEnabled: state.exchangeRatesReducer.isFilterEnabled,
})

const mapDispatchToProps = (dispatch) => ({
  toggleFilter: () => dispatch(toggleFilter()),
  updateFilterValue: (value) => dispatch(updateFilterValue(value)),
})

class CurrencyDashboard extends Component {
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
        this.apiGetRates()
        this._getRatesInterval = setInterval(this.apiGetRates, configReloadInterval)
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
      isLoading: false,
    })
  }

  render () {
    const {filterValue, isFilterEnabled, toggleFilter, updateFilterValue} = this.props

    return (
      <CurrencyDashboardView
          {...this.state}
          filterValue={filterValue}
          isFilterEnabled={isFilterEnabled}
          toggleFilter={toggleFilter}
          updateFilterValue={updateFilterValue}
      />
    )
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps,
)(CurrencyDashboard)
