import React from 'react'
import './styles.module.css'
import Emoji from '../emoji/emoji.component'

type Props = {
  currentRate: string,
  previousRate: string,
  trend: "N/A" | "stagnating" | "growing" | "declining"
}

const RateTrends: React.FC<Props> = ({currentRate, trend}) => {
  let icon;
  if(trend === "N/A" || trend === "stagnating") icon = "➡️"
  if(trend === "growing") icon = "↗️"
  if(trend === "declining") icon = "↘️"
  return (
    <>
      <td scope="col">{currentRate}</td>
      <td scope="col">{trend} <Emoji label="trend" symbol={icon}/> </td>
    </>
  )
}

export default RateTrends