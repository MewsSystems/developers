import React, { useEffect } from 'react'
import { connect } from 'react-redux'
import { isEmpty, keys } from 'ramda'

import { fetchConfigInit } from '../store/actions'
import Currency from './Currency'
import Trend from './Trend'
import Filter from './Filter'
import {
  ListContainer,
  FilterContainer,
  CurrenciesContainer,
  CurrencyHeader,
  RatesHeader,
  SingleCurrency,
} from '../assets/Styles'
import Spinner from '../assets/UI/Spinner'
import { CurrencyListProps, CurrencyListState } from '../types'

const CurrencyList: React.FC<CurrencyListProps> = ({
  fetchConfigInit,
  rates,
  config,
  filtered,
}) => {
  useEffect(() => {
    fetchConfigInit()
  }, [fetchConfigInit])

  const renderList = () => {
    if (!isEmpty(config)) {
      const filterApplied = filtered.length
      let CurrencyKeys = filterApplied ? filtered : keys(config)
      return (
        <>
          <CurrencyHeader>Currency</CurrencyHeader>
          <RatesHeader>Rate</RatesHeader>
          {CurrencyKeys.map((cur: string) => (
            <SingleCurrency key={cur}>
              <Currency currency={config[cur]} />
              <Trend rate={rates[cur]} />
            </SingleCurrency>
          ))}
        </>
      )
    }
    return <Spinner />
  }
  return (
    <ListContainer>
      <FilterContainer>
        <Filter config={config} />
      </FilterContainer>
      <CurrenciesContainer>{renderList()}</CurrenciesContainer>
    </ListContainer>
  )
}
const mapStateToProps = (state: CurrencyListState) => {
  return {
    config: state.config,
    rates: state.rates,
    filtered: state.filtered,
  }
}

export default connect(
  mapStateToProps,
  { fetchConfigInit }
)(CurrencyList)
