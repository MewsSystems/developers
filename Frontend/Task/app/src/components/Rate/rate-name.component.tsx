import React from 'react'
import './styles.module.css'

type Props = {
  name: string,
  code: string,
}

const RateName: React.FC<Props> = ({name, code}) => {
  return (
    <>
    <td scope="col">{name}</td>
    <td scope="col">{code}</td>
    </>
  )
}

export default RateName