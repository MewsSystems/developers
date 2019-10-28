import React from 'react'
import styled, { createGlobalStyle } from 'styled-components'
import { node } from 'prop-types'
import { ToastContainer } from 'react-toastify'
import 'react-toastify/dist/ReactToastify.css'

import { PINK, TURQUOISE } from '../../constants'

const GlobalStyle = createGlobalStyle`
  * {
  font-family: 'Lato', sans-serif;
  }

  html, body {
    height: 100%;
    overflow: hidden;
    }
    
  body {

     background: url('https://image.freepik.com/free-vector/seamless-pattern_1159-5086.jpg') repeat;

     > div {
       height: 100%;
      }
    }
`

const ContentContainer = styled.div`
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
`

const WhiteBackgroundContainer = styled.div`
  align-items: center;
  background: radial-gradient(
    ellipse at center,
    rgba(255, 255, 255, 1) 0%,
    rgba(255, 255, 255, 0.3) 40%,
    rgba(255, 255, 255, 0) 100%
  );

  display: flex;
  height: 1000px;
  justify-content: center;
  width: 1000px;
`

const Layout = ({ children }) => (
  <ContentContainer>
    <GlobalStyle />
    <WhiteBackgroundContainer>{children}</WhiteBackgroundContainer>
    <ToastContainer
      autoClose={3000}
      closeOnClick={true}
      pauseOnHover={true}
      position="bottom-left"
      progressStyle={{
        backgroundImage: `linear-gradient(90deg, ${TURQUOISE} 0%, ${PINK} 100%)`,
        height: '10px',
      }}
    />
  </ContentContainer>
)

Layout.propTypes = {
  children: node.isRequired,
}

export default Layout
