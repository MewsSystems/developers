import React from 'react'

import { CurrencyItem } from '../assets/Styles'
import { CurrencyArray } from '../types'

const Currency: React.FC<CurrencyArray> = ({ currency }) => {
  return (
    <CurrencyItem>{`${currency[0].name}/${currency[1].name}`}</CurrencyItem>
  )
}

export default Currency
