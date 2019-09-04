import React, { useEffect, useState } from 'react';
import axios from 'axios'
import { connect } from 'react-redux'
import { generateCurrencyPairs } from 'helperFunctions/generateCurrencyPairs'
import { addPair, filterPair } from 'actions/currencyPairs'
import CurrencyPairsRateList from 'components/CurrencyPairsRateList'

const CurrencyPairsSelector = ({ addPairAction, filterPairAction, currencyPairs }) => {

  const defaultInitialText = 'Fetching currency pairs...'

  const [initialText, setInitialText] = useState(defaultInitialText)
  const [allCurrencyPairs, setAllCurrencyPairs] = useState([])
  const [pairsToFilter, setPairsToFilter] = useState([])

  const showAllCurrencyPairs = (currencyPairs) => {
    return currencyPairs.map(pair => (
      <label key={pair.name}>
        {pair.name}
        <input
          type="checkbox"
          name={pair.name}
          onChange={(e) => filterCurrencyPairs(e)}
        />
      </label>
    ))
  }

  const filterCurrencyPairs = (e) => {
    const pair = e.target.name
    setPairsToFilter((prevValue) => [...prevValue, pair])
  }

  const renderCurrencyPairsRateList = (currencyPairs) => {
    return currencyPairs
      .filter((pair) => pair.display === true)
      .map((pair) => <CurrencyPairsRateList key={pair.name} currencyPair={pair} />)
  }

  useEffect(() => {
    const fetchConfiguration = async () => {
      try {
        const { data } = await axios.get('http://localhost:3000/configuration')
        const currencyPairs = generateCurrencyPairs(data.currencyPairs, addPairAction)
        setAllCurrencyPairs(currencyPairs)
        setInitialText('Filter currency pairs:')
      }

      catch (error) {
        setInitialText('Could not fetch the currency data. Please try again...')
      }
    }
    fetchConfiguration()

  }, [])

  const handleClick = () => {
    setPairsToFilter([])
    pairsToFilter.map(pair => filterPairAction(pair))
  }

  return (
    <div>
      {initialText}
      {allCurrencyPairs && <form>{showAllCurrencyPairs(allCurrencyPairs)}</form>}
      {renderCurrencyPairsRateList(currencyPairs)}
      <button
        disabled={initialText === defaultInitialText ? true : false}
        onClick={() => handleClick()}>Fetch rates</button>
    </div>
  )
}

const mapStateToProps = ({ currencyPairs }) => ({
  currencyPairs
})

const mapDispatchToProps = (dispatch) => ({
  addPairAction: (pair) => dispatch(addPair(pair)),
  filterPairAction: (pair) => dispatch(filterPair(pair)),
})

export default connect(mapStateToProps, mapDispatchToProps)(CurrencyPairsSelector)
