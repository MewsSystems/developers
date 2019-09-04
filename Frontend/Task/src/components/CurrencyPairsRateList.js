import React, { useEffect, useState } from 'react';
import axios from 'axios'
import { computeTrend } from 'helperFunctions/computeTrend'
import { interval } from 'config.json'

const CurrencyPairsRateList = ({ currencyPair }) => {
  const { name, id } = currencyPair

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
    <section className="rates__item">
      <p className="rates__content content__text">{name}</p>
      <p className="rates__content content__text">{Math.round(currentPairRate * 100) / 100}</p>
      <p className="rates__content content__text">{trend}</p>
      {isError && <p className="rates__content rates__error content__text">Server error! Your rate is not up to date.</p>}
    </section>
  )
}

export default CurrencyPairsRateList
