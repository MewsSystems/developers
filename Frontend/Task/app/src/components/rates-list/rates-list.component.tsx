import React, {useEffect} from 'react'
import { connect } from 'react-redux'
import { ConfigDispatch, ConfigReducerState } from '../../redux/configuration/configuration.models'
import { fetchConfigAsync } from '../../redux/configuration/configuration.actions'
import { searchCurrency } from '../../redux/filter/filter.actions'
import { fetchRatesAsync } from '../../redux/rates/rates.actions'
import { RateReducerState } from '../../redux/rates/rates.model'
import { getFilteredCurrencies } from '../../redux/filter/filter.selectors'
import {namesArray, saveState, loadState } from '../../utils'
import Alert from '../alert/alert.component'
import Select from '../select/select.component'
import TableHeader from './table-header'
import TableBody from './table-body'
import { RootState } from '../../types'
import './styles.module.css'
import { toast } from 'react-toastify';
import{ WithSpinnerBody} from '../with-spinner/with-spinner.component'

const TableBodyWithSpinner = WithSpinnerBody(TableBody)

type Props = {
  fetchConfig: Function,
  fetchRates: Function,
  searchCurrency: Function,
  rates: RateReducerState,
  config: ConfigReducerState,
  isError: boolean,
  loadingConfig: boolean,
  loadingRates: boolean
}

const RatesList: React.FC<Props> = (props) => {
  const {fetchConfig, fetchRates, rates, config, searchCurrency, isError, loadingConfig, loadingRates, searchTerm} = props

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

  const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    saveState("select", e.target.value)
    searchCurrency(e.target.value)

  }

  const notify = () => toast.error('500 Internal Server Error!!');
  const renderOptions = namesArray.map(cur => {
    return <option selected={cur.value === loadState("select")} key={cur.name} value={cur.value}>{cur.name}</option>
  })
  return (
    <>
      <Alert/>
      <Select
        handleChange={handleChange}
        value={searchTerm}
        options={renderOptions}
      />
     <table className="table">
        <TableHeader/>
        <TableBodyWithSpinner
            config={config}
            rates={rates}
            isLoading={loadingConfig}
            loadingRates={loadingRates}
        />
      </table>
    </>
  )
}

const mapStateToProps = (state: RootState) => {
  return {
    config: getFilteredCurrencies(state),
    loadingConfig: state.configuration.isLoading,
    rates: state.rates.ratesList,
    loadingRates: state.rates.isLoading,
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