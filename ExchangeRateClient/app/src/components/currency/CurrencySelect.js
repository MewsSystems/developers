import React from 'react'

export default function CurrencySelect({ value, handleChange, currencyPairs }) {

  const keys = Object.keys(currencyPairs);
  const values = Object.values(currencyPairs);
  const pairs = [];
  keys.forEach(function (item, key) {
    pairs.push({
      key: item,
      value: values[key]
    })

  });
  return (
    <select value={value} onChange={(event) => handleChange(event.target.value)}>
      <option key={'0'} value={'_none'}>Select Currency </option>
      {pairs.map((pair, index) => (
        <option key={pair.key} value={pair.key}>{pair.value[0].name} to {pair.value[1].name}</option>
      ))}
    </select>
  )
}
