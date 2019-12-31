import React, {useEffect} from 'react'
import { connect } from 'react-redux'
import { ConfigDispatch, ConfigReducerState } from '../../redux/configuration/configuration.models'
import { fetchConfigAsync } from '../../redux/configuration/configuration.actions'
import { fetchRatesAsync } from '../../redux/rates/rates.actions'
import {RateReducerState} from '../../redux/rates/rates.model'
import { Table } from 'reactstrap'
import Rate from '../Rate/rate.component'
import RateName from '../Rate/rate-name.component'
import RateTrends from '../Rate/rate-trends.component'
import {RootState} from '../../types'
import './styles.module.css'
import Spinner from 'react-spinkit'
import WithSpinner from '../hoc/spinner.component'
import Select from 'react-select'

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
  rates: RateReducerState,
  config: ConfigReducerState,
}

const RatesList: React.FC<Props> = (props) => {

  const {fetchConfig, fetchRates, rates, config} = props
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
  return (
    <>
    <div className="select-wrapper">
      <Select/>
    </div>
     <Table className="table">
     <TableHeader/>
        <tbody>
          {Object.keys(config).map(id => {
            return (
              <Rate key={config[id].name}>
                <RateName
                  name={config[id].name}
                  code={config[id].code}
                />
                {
                rates[id] ?
                (<RateTrends
                  currentRate={rates[id].currentRate}
                  previousRate={rates[id].previousRate}
                  trend={rates[id].trend}
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
    config: state.configuration.currencies,
    rates: state.rates.ratesList,
    isLoading: state.configuration.isLoading
  }
}

const mapDispatchToProps = (dispatch: ConfigDispatch) => {
  return {
    fetchConfig: () => dispatch(fetchConfigAsync()),
    fetchRates: () => dispatch(fetchRatesAsync())
  }
}
const RatesListWithSpinner = WithSpinner(RatesList)
export default connect(mapStateToProps, mapDispatchToProps)(RatesList);