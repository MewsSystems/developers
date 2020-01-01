import React, {useEffect, useState} from 'react'
import { connect } from 'react-redux'
import { ConfigDispatch, ConfigReducerState } from '../../redux/configuration/configuration.models'
import { fetchConfigAsync } from '../../redux/configuration/configuration.actions'
import { searchCurrency } from '../../redux/filter/filter.actions'
import { fetchRatesAsync } from '../../redux/rates/rates.actions'
import { RateReducerState } from '../../redux/rates/rates.model'
import { getFilteredCurrencies } from '../../redux/filter/filter.selectors'
import Rate from '../Rate/rate.component'
import RateName from '../Rate/rate-name.component'
import RateTrends from '../Rate/rate-trends.component'
import Alert from '../alert/alert.component'
import Input from '../form-input/form-input.component'
import { RootState } from '../../types'
import './styles.module.css'
import Spinner from 'react-spinkit'
import { toast } from 'react-toastify';

const TableHeader = () => (
  <thead>
    <tr>
      <th scope="col">Name</th>
      <th scope="col">Code</th>
      <th scope="col">Current value</th>
      <th scope="col">Trend</th>
    </tr>
  </thead>
)

type Props = {
  fetchConfig: Function,
  fetchRates: Function,
  searchCurrency: Function,
  rates: RateReducerState,
  config: ConfigReducerState,
  isError: boolean
}

const RatesList: React.FC<Props> = (props) => {
  const {fetchConfig, fetchRates, rates, config, searchCurrency, isError} = props

  useEffect(() =>{
    const { fetchConfig } = props
    fetchConfig()
  }, [fetchConfig])

  useEffect(() => {
      const interval = setInterval(() => {
        fetchRates()
      }, 10000)
      isError && notify()
    return () => clearInterval(interval)

  }, [fetchRates, isError])

  const handleChange = (value: string) => {
    searchCurrency(value)
  }

  const notify = () => toast.error('500 Internal Server Error!!');

  return (
    <>
      <Alert/>
      <Input
        type="search"
        handleChange={handleChange}
        className="input-search"
        placeholder="Search by name"
      />
     <table className="table">
     <TableHeader/>
        <tbody>
          {config ? (config.map(rate => {
            return (
              <Rate key={rate.name}>
                <RateName
                  name={rate.name}
                  code={rate.code}
                />
                {
                rates[rate.id] ?
                (<RateTrends
                  currentRate={rates[rate.id].currentRate}
                  trend={rates[rate.id].trend}
                  />)
                  :
                  (<Spinner className="spinner" name="circle"/>)
                }
              </Rate>
            )
          })) : (<Spinner name="ball-spin-fade-loader" />)}
        </tbody>
      </table>
    </>

  )
}

const mapStateToProps = (state: RootState) => {
  return {
    config: getFilteredCurrencies(state),
    rates: state.rates.ratesList,
    isError: state.rates.showErrorAlert,
    searchTerm: state.rates.searchTerm
  }
}

const mapDispatchToProps = (dispatch: ConfigDispatch) => {
  return {
    fetchConfig: () => dispatch(fetchConfigAsync()),
    fetchRates: () => dispatch(fetchRatesAsync()),
    searchCurrency: (value: string) => dispatch(searchCurrency(value))
  }
}
export default connect(mapStateToProps, mapDispatchToProps)(RatesList);