import React, { useEffect, useState } from 'react';
import axios from 'axios'
import { connect } from 'react-redux'
import { generateCurrencyPairs } from 'helperFunctions/generateCurrencyPairs'
import { getStoragedData } from 'helperFunctions/getStoragedData'
import { setStorage } from 'helperFunctions/setStorage'
import { addPair, setDisplay } from 'actions/currencyPairs'
import { addToFilters, removeFromFilters } from 'actions/filters'
import CurrencyPairsRateList from 'components/CurrencyPairsRateList'

const CurrencyPairsSelector = ({ addPairAction, addToFiltersAction, currencyPairs, filters, setDisplayAction, removeFromFiltersAction }) => {

  const defaultInitialText = 'Fetching currency pairs...'

  const [initialText, setInitialText] = useState(defaultInitialText)
  const [allPossibleCurrencyPairs, setAllPossibleCurrencyPairs] = useState([])

  useEffect(() => {
    const updateStates = (configData, action) => {
      const currencyPairs = generateCurrencyPairs(configData, action)
      setAllPossibleCurrencyPairs(currencyPairs)
      setInitialText('Filter currency pairs:')
    }

    const fetchConfiguration = async () => {
      if (getStoragedData('currencyPairs') !== null) {
        return updateStates(getStoragedData('currencyPairs'), addPairAction)
      }

      try {
        const { data } = await axios.get('http://localhost:3000/configuration')
        setStorage('currencyPairs', data.currencyPairs)
        updateStates(data.currencyPairs, addPairAction)
      }
      catch (error) {
        setInitialText('Could not fetch the currency data. Please try again...')
      }
    }


    const fetchFilters = () => {
      if (getStoragedData('currencyPairs') !== null) {
        const storagedFilters = getStoragedData('filters')
        storagedFilters.forEach((filter) => addToFiltersAction(filter))
      }
    }

    fetchConfiguration()
    fetchFilters()

  }, [])


  useEffect(() => {
    setStorage('filters', filters)
  }, [filters])

  const handleCheckboxChange = (e) => {
    const pair = e.target.name
    e.target.checked ? addToFiltersAction(pair) : removeFromFiltersAction(pair)
  }

  const handleBtnClick = () => {
    setDisplayAction(filters)
  }

  const renderAllPossiblePairs = (currencyPairs) => {
    return currencyPairs.map(pair => (
      <label key={pair.name}>
        {pair.name}
        <input
          type="checkbox"
          name={pair.name}
          defaultChecked={filters.includes(pair.name)}
          onChange={(e) => handleCheckboxChange(e)}
        />
      </label>
    ))
  }

  const renderCurrencyPairsRateList = (currencyPairs) => {
    return currencyPairs
      .filter((pair) => pair.display === true)
      .map((pair) => <CurrencyPairsRateList key={pair.name} currencyPair={pair} />)
  }

  return (
    <div>
      {initialText}
      {allPossibleCurrencyPairs && <form>{renderAllPossiblePairs(allPossibleCurrencyPairs)}</form>}
      {renderCurrencyPairsRateList(currencyPairs)}
      <button
        disabled={initialText === defaultInitialText ? true : false}
        onClick={() => handleBtnClick()}>Fetch rates</button>
    </div>
  )
}

const mapStateToProps = ({ currencyPairs, filters }) => ({
  currencyPairs,
  filters,
})

const mapDispatchToProps = (dispatch) => ({
  addPairAction: (pair) => dispatch(addPair(pair)),
  addToFiltersAction: (pair) => dispatch(addToFilters(pair)),
  removeFromFiltersAction: (pair) => dispatch(removeFromFilters(pair)),
  setDisplayAction: (pair) => dispatch(setDisplay(pair)),
})

export default connect(mapStateToProps, mapDispatchToProps)(CurrencyPairsSelector)
