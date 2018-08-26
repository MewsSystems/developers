import React from 'react'
import RatesTableRow from './RatesTableRow'
import styles from '../styles/ratesTable.css'

export default ({pairs}) => {
  return (
  <table className={styles.ratesTable}>
    <tbody>
      <tr>
        <th>Name</th>
        <th>Rate</th>
        <th>Trend</th>
      </tr>
      {Object.keys(pairs).filter(o => pairs[o].selected)
        .map(o => <RatesTableRow {...pairs[o]} key={o} />)}
      </tbody>
  </table>
)}