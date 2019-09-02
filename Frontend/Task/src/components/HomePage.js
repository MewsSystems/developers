import React, { useEffect, useState } from 'react';
import axios from 'axios'
import { renderCurrencyPairs } from 'helperFunctions/renderCurrencyPairs'

const HomePage = () => {

  const [welcomeText, setWelcomeText] = useState('Fetching currency pairs...')


  useEffect(() => {
    axios.get('http://localhost:3000/configuration')
      .then(({ data }) => {
        const currencyData = data.currencyPairs
        const currencyPairs = renderCurrencyPairs(currencyData)
        return currencyPairs
      })
      .then((currencyPairs) => setWelcomeText(currencyPairs))
      .catch((error) => setWelcomeText('Could not fetch the currency data. Please try again...'))
  }, [])

  return (
    <div>
      {/* {currencyPairs ? welcomeText : currencyPairs} */}
      {welcomeText}
    </div>
  )
}

export default HomePage;
