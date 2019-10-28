import React from 'react'
import styled, { keyframes } from 'styled-components'

import { PURPLE } from '../../../constants'

const LOADING = keyframes`
	0% {
		transform: rotate(0deg);
	}
	100% {
		transform: rotate(360deg);
	}
`

const StyledLoader = styled.div`
  animation: ${LOADING} 1s linear infinite;
  border-radius: 50%;
  border: 5px solid lightgrey;
  border-top: 5px solid ${PURPLE};
  height: 40px;
  margin: 0 auto;
  position: relative;
  width: 40px;
`

const Loader = () => <StyledLoader />

export default Loader
