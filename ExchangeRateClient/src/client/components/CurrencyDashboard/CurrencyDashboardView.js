import React from 'react'
import PropTypes from 'prop-types'

import CurrencyPairsRateList from './CurrencyPairsRateList'
import CurrencyPairsSelector from './CurrencyPairsSelector'

const styles = {}

const CurrencyDashboardView = ({
  isLoading,
  error,
  errorMessage,
  data,
  rates,
  updateFilterValue,
  toggleFilter,
  isFilterEnabled,
  filterValue,
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
    <div className={styles['currency-dashboard']}>
      <CurrencyPairsSelector
          data={data}
          updateFilterValue={updateFilterValue}
          toggleFilter={toggleFilter}
          isFilterEnabled={isFilterEnabled}
          filterValue={filterValue}
      />
      <CurrencyPairsRateList
          data={data}
          rates={rates}
          filterValue={filterValue}
          updateFilterValue={updateFilterValue}
          isFilterEnabled={isFilterEnabled}
      />
    </div>
  )
}

CurrencyDashboardView.propTypes = {
  isLoading: PropTypes.bool,
  error: PropTypes.bool,
  errorMessage: PropTypes.string,
  data: PropTypes.object.isRequired,
  rates: PropTypes.object,
  updateFilterValue: PropTypes.func,
  toggleFilter: PropTypes.func,
  isFilterEnabled: PropTypes.bool,
  filterValue: PropTypes.array,
}

export default CurrencyDashboardView
