import React from 'react'
import './styles.module.css'

const Input = ({handleChange, className, ...otherProps}) => {
  return(
     <input
        className={className}
        onChange={(e) => handleChange(e.target.value)}
        {...otherProps}
     />
  )
}

export default Input;