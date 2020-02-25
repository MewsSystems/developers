import * as React from 'react'
import { keyframes } from '@emotion/core'
import { transparentize } from 'polished'
import styled from '../utils/styled'

export const LoadingSpinner: React.SFC = () => (
  <Spinner>
    <div className="cube1" />
    <div className="cube2" />
  </Spinner>
)

const CubeMove = keyframes`
  25% {
    transform: translateX(42px) rotate(-90deg) scale(0.5);
    -webkit-transform: translateX(42px) rotate(-90deg) scale(0.5);
  } 50% {
    transform: translateX(42px) translateY(42px) rotate(-179deg);
    -webkit-transform: translateX(42px) translateY(42px) rotate(-179deg);
  } 50.1% {
    transform: translateX(42px) translateY(42px) rotate(-180deg);
    -webkit-transform: translateX(42px) translateY(42px) rotate(-180deg);
  } 75% {
    transform: translateX(0px) translateY(42px) rotate(-270deg) scale(0.5);
    -webkit-transform: translateX(0px) translateY(42px) rotate(-270deg) scale(0.5);
  } 100% {
    transform: rotate(-360deg);
    -webkit-transform: rotate(-360deg);
  }
`

const Spinner = styled('div')`
  margin: 100px auto;
  width: 40px;
  height: 40px;
  position: relative;

  & .cube1,
  & .cube2 {
    background-color: ${props => props.theme.colors.brand};
    width: 15px;
    height: 15px;
    position: absolute;
    top: 0;
    left: 0;

    -webkit-animation: ${CubeMove} 1.8s infinite ease-in-out;
    animation: ${CubeMove} 1.8s infinite ease-in-out;
  }

  & .cube2 {
    -webkit-animation-delay: -0.9s;
    animation-delay: -0.9s;
  }
`

export const LoadingOverlay = styled('div')`
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  z-index: 10;
  background: ${props => transparentize(0.25, props.theme.colors.background)};
`

export const LoadingOverlayInner = styled('div')`
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 200px;
`

type LoadingProps = {
  loading: boolean
}

export const Loading = ({ loading }: LoadingProps) => {
  if (!loading) return null

  return (
    <LoadingOverlay>
      <LoadingOverlayInner>
        <LoadingSpinner />
      </LoadingOverlayInner>
    </LoadingOverlay>
  )
}
