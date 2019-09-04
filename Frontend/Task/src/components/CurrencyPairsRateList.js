import React, { useEffect, useState } from 'react';
import axios from 'axios'
import { computeTrend } from 'helperFunctions/computeTrend'
import { interval } from 'config.json'

const CurrencyPairsRateList = ({ currencyPair }) => {
  const { display: shouldDisplayRate, name, id } = currencyPair

  const [currentPairRate, setCurrentPairRate] = useState(null)
  const [isError, setIsError] = useState(false)
  const [trend, setTrend] = useState(null)


  useEffect(() => {
    const fetchRate = async () => {

      try {
        const { data } = await axios.get(`http://localhost:3000/rates?currencyPairIds[]=${id}`)
        const newestRate = data.rates[id]

        setCurrentPairRate((prevValue) => {
          const rates = {
            prevValue,
            currentValue: newestRate,
          }
          computeTrend({ ...rates }, setTrend)
          return newestRate
        })
        setIsError(false)
      }

      catch (error) {
        setIsError(true);
      }
    }


    if (currentPairRate === null) {
      fetchRate()
    }
    const intervalId = setInterval(() => {
      fetchRate()
    }, interval)

    return () => clearInterval(intervalId)


  }, [currentPairRate])


  return (
    <>
      {shouldDisplayRate && <div>
        {shouldDisplayRate && <p>{name}</p>}
        {shouldDisplayRate && <p>{currentPairRate}</p>}
        {shouldDisplayRate && <p>{trend}</p>}
        {isError && <p>Server error! Your rate is not up to date.</p>}
      </div>}
    </>
  )
}

export default CurrencyPairsRateList
