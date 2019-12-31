import React from 'react'
import './styles.module.css'
import Emoji from '../emoji/emoji.component'

const RateTrends = ({currentRate, previousRate, trend}) => {
  let icon;
  if(trend === "N/A") icon = "➡️"
  if(trend === "growing") icon = "↗️"
  if(trend === "declining") icon = "↘️"
  if(trend === "stagnating") icon = "➡️"
  return (
    <>
      <td scope="col">{currentRate}</td>
      <td scope="col">{previousRate}</td>
      <td scope="col">{trend} <Emoji symbol={icon}/> </td>
    </>
  )
}

export default RateTrends