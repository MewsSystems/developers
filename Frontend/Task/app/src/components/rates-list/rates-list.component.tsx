import React, {useEffect} from 'react'
import { connect } from 'react-redux'
import { ConfigDispatch, ConfigReducerState } from '../../redux/configuration/configuration.models'
import { fetchConfigAsync } from '../../redux/configuration/configuration.actions'
import { searchCurrency } from '../../redux/filter/filter.actions'
import { fetchRatesAsync } from '../../redux/rates/rates.actions'
import { RateReducerState } from '../../redux/rates/rates.model'
import { getFilteredCurrencies } from '../../redux/filter/filter.selectors'
import { Table } from 'reactstrap'
import Rate from '../Rate/rate.component'
import RateName from '../Rate/rate-name.component'
import RateTrends from '../Rate/rate-trends.component'
import { RootState } from '../../types'
import './styles.module.css'
import Spinner from 'react-spinkit'

const TableHeader = () => (
  <thead>
    <tr>
      <th scope="col">Name</th>
      <th scope="col">Code</th>
      <th scope="col">Current value</th>
      <th scope="col">Previous value</th>
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
}

const RatesList: React.FC<Props> = (props) => {
  const {fetchConfig, fetchRates, rates, config, searchCurrency} = props

  useEffect(() =>{
    const { fetchConfig } = props
    fetchConfig()
  }, [fetchConfig])

  useEffect(() => {
      const interval = setInterval(() => {
        fetchRates()
      }, 10000)
    return () => clearInterval(interval)

  }, [fetchRates])

  const handleChange = (value: string) => {
    searchCurrency(value)
  }
  return (
    <>
    <div className="select-wrapper">
     <input
        type="search"
        className="input-search"
        onChange={(e) => handleChange(e.target.value)}
        placeholder="Search by name..."
     />
    </div>
     <Table className="table">
     <TableHeader/>
        <tbody>
          {config.map(rate => {
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
                  previousRate={rates[rate.id].previousRate}
                  trend={rates[rate.id].trend}
                  />)
                  :
                  (<Spinner className="spinner" name="circle"/>)
                }
              </Rate>
            )
          })}
        </tbody>
      </Table>
    </>

  )
}

const mapStateToProps = (state: RootState) => {
  return {
    config: getFilteredCurrencies(state),
    rates: state.rates.ratesList,
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