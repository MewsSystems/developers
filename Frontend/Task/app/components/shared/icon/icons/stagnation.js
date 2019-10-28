import React from 'react'
import { string } from 'prop-types'

const Stagnation = ({ color }) => (
  <svg
    fill={color}
    width="100%"
    height="100%"
    viewBox="0 0 372 372"
    xmlns="http://www.w3.org/2000/svg"
  >
    <path d="M101.98,186.441H30.484c-5.523,0-10,4.477-10,10v166.021c0,5.523,4.477,10,10,10h71.496c5.523,0,10-4.477,10-10V196.441    C111.98,190.919,107.503,186.441,101.98,186.441z" />
    <path d="M221.98,0h-71.496c-5.523,0-10,4.477-10,10v352.465c0,5.521,4.477,10,10,10h71.496c5.523,0,10-4.479,10-10V10    C231.98,4.477,227.504,0,221.98,0z" />
    <path d="M341.98,115.257h-71.496c-5.523,0-10,4.477-10,10v237.208c0,5.521,4.477,10,10,10h71.496c5.523,0,10-4.479,10-10V125.257    C351.98,119.734,347.504,115.257,341.98,115.257z" />
  </svg>
)

Stagnation.propTypes = {
  color: string.isRequired,
}

export default Stagnation
