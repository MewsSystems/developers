import React from 'react'
import './styles.module.css'

const RateName = ({name, code}) => {
  return (
    <>
    <td scope="col">{name}</td>
    <td scope="col">{code}</td>
    </>
  )
}

export default RateName