import React from 'react'
import './styles.module.css'

const Select = ({handleChange, value, options, ...otherProps}) => {
  return(
   <select
      className="select"
      value={value}
      onChange={(e) => handleChange(e)}
      {...otherProps}
   >
   {options}
   </select>
  )
}

export default Select;