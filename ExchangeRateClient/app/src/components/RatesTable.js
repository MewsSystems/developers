import React from 'react'
import RatesTableRow from './RatesTableRow'
import styles from '../styles/ratesTable.css'
import { _map, _filter } from '../utils/lodash'


const filtered = (pairs) => _filter({'selected': true}, pairs)
const mapper = (pair, id) => <RatesTableRow  key={id} {...pair}/>

export default ({pairs}) => {
  return (
  <table className={styles.ratesTable}>
    <tbody>
      <tr>
        <th>Name</th>
        <th>Rate</th>
        <th>Trend</th>
      </tr>
      {_map(mapper, filtered(pairs))}
      </tbody>
  </table>
)}