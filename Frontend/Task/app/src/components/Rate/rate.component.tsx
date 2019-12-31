import React from 'react'
import './styles.module.css'

type Props = {
  children: React.ReactNode
}

const Rate: React.FC<Props>  = ({children}) => {
  return <tr>{children}</tr>
}

export default Rate