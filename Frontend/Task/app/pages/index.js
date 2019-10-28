import React, { useEffect } from 'react'
import { connect } from 'react-redux'
import { arrayOf, bool, func, oneOf, string } from 'prop-types'

import { fetchState, UPDATE_INTERVAL } from '../constants'
import { getPairs, getRates } from '../actions'
import {
  getCurrencyPairIds,
  getPairListState,
  getRateListState,
  getInitialLoadSuccessful,
} from '../selectors'

import CurrencyDisplay from '../components/currency-display'
import Loader from '../components/shared/loader'
import Layout from '../components/layout'

const HomePage = ({
  currencyPairIds,
  getPairs,
  pairListState,
  getRates,
  isRatesLoaded,
}) => {
  useEffect(() => {
    if (!pairListState) {
      getPairs()
    }
  }, [])

  useEffect(() => {
    let rateUpdateInterval

    if (pairListState === fetchState.DONE) {
      getRates(currencyPairIds)
      rateUpdateInterval = setInterval(() => {
        getRates(currencyPairIds)
      }, UPDATE_INTERVAL)
    }

    return () => clearInterval(rateUpdateInterval)
  }, [pairListState])

  return <Layout>{isRatesLoaded ? <CurrencyDisplay /> : <Loader />}</Layout>
}

HomePage.propTypes = {
  currencyPairIds: arrayOf(string).isRequired,
  getPairs: func.isRequired,
  getRates: func.isRequired,
  isRatesLoaded: bool.isRequired,
  pairListState: oneOf(Object.keys(fetchState)),
}

const mapStateToProps = state => ({
  currencyPairIds: getCurrencyPairIds(state),
  isRatesLoaded: getInitialLoadSuccessful(state),
  pairListState: getPairListState(state),
  rateListState: getRateListState(state),
})

const mapDispatchToProps = dispatch => ({
  getPairs: getPairs(dispatch),
  getRates: getRates(dispatch),
})

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(HomePage)
