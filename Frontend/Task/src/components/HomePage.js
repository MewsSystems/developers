import React, { useEffect, useState } from 'react';
import axios from 'axios'
import { connect } from 'react-redux'
import { generateCurrencyPairs } from 'helperFunctions/generateCurrencyPairs'
import { addPair, filterPair } from 'actions/currencyPairs'
import CurrencyPairsRateList from 'components/CurrencyPairsRateList'

const HomePage = ({ addPairAction, filterPairAction, filteredCurrencyPairs }) => {

  const [welcomeText, setWelcomeText] = useState('Fetching currency pairs...')
  const [currencyPairs, setCurrencyPairs] = useState([])

  const renderCurrencyPairs = (currencyPairs) => {
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
    filterPairAction(e.target.name)
  }

  const renderFilteredCurrencyPairs = (filteredCurrencyPairs) => {
    return filteredCurrencyPairs.map((pair) => <CurrencyPairsRateList key={pair.name} currencyPair={pair} />)
  }

  useEffect(() => {
    const fetchData = async () => {
      try {
        const { data } = await axios.get('http://localhost:3000/configuration')
        const currencyPairs = generateCurrencyPairs(data.currencyPairs, addPairAction)
        setCurrencyPairs(currencyPairs)
        setWelcomeText('Filter currency pairs:')
      }

      catch (error) {
        setWelcomeText('Could not fetch the currency data. Please try again...')
      }
    }
    fetchData()

  }, [])

  return (
    <div>
      {welcomeText}
      {currencyPairs && <form>{renderCurrencyPairs(currencyPairs)}</form>}
      {renderFilteredCurrencyPairs(filteredCurrencyPairs)}
    </div>
  )
}

const mapStateToProps = ({ currencyPairs }) => ({
  filteredCurrencyPairs: currencyPairs
})

const mapDispatchToProps = (dispatch) => ({
  addPairAction: (pair) => dispatch(addPair(pair)),
  filterPairAction: (pair) => dispatch(filterPair(pair)),
})

export default connect(mapStateToProps, mapDispatchToProps)(HomePage)
