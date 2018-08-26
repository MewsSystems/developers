import React from 'react';
import styles from '../styles/ratesTable.css'
import Loader from './Loader'

export default ({baseCode, baseName, secondaryCode, secondaryName, oldRate, newRate}) => {
  var trend = oldRate !== null && newRate !== null ? newRate - oldRate : null

  const rateContent = newRate === null ? <Loader /> : newRate;
  const trendContent = newRate === null || oldRate === null ? 
  <Loader /> : trend === 0 ? 'stagnating' : trend > 0 ? 'growing' : 'declining'

  return(
  <tr>
    <td className={styles.name}>{`${baseCode}/${secondaryCode}`}</td>
    <td className={styles.rate}>{rateContent}</td>
    <td className={styles.trend}>{trendContent}</td>
  </tr>
)}