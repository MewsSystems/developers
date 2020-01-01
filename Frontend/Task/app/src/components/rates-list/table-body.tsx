import React from 'react'
import Rate from '../rate/rate.component'
import RateName from '../rate/rate-name.component'
import RateTrends from '../rate/rate-trends.component'
import{ WithSpinnerRate} from '../with-spinner/with-spinner.component'
import {loadState} from '../../utils'

const RateTrendsWithSpinner = WithSpinnerRate(RateTrends)

const TableBody = ({config, rates, loadingRates}) => {
  return (
    <tbody>
    {config.map(rate => {
        return (
          <Rate key={rate.name}>
            <RateName
              name={rate.name}
              code={rate.code}
            />
            <RateTrendsWithSpinner
              isLoading={loadingRates}
              currentRate={rates[rate.id] && rates[rate.id].currentRate}
              trend={rates[rate.id] && rates[rate.id].trend}
            />
          </Rate>
        )
      }
    )}
  </tbody>
  )
}

export default TableBody