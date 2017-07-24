import React from 'react'
import PropTypes from 'prop-types'

import CurrencyPairsRateList from './CurrencyPairsRateList'

const CurrencyDashboardView = ({
  isLoading,
  error,
  errorMessage,
  data,
  rates,
}) => {
  if (error) {
    return (
      <div>{`An error occured: ${errorMessage || 'Try reloading your browser!'}`}</div>
    )
  }

  if (isLoading) {
    return (
      <div>Loading...</div>
    )
  }

  return (
    <CurrencyPairsRateList
        data={data}
        rates={rates}
    />
  )
}

CurrencyDashboardView.propTypes = {
  isLoading: PropTypes.bool,
  error: PropTypes.bool,
  errorMessage: PropTypes.string,
  data: PropTypes.object.isRequired,
  rates: PropTypes.object,
}

export default CurrencyDashboardView
