import styled from 'styled-components'

export const ChartWrapper = styled.div`
  position: relative;
  max-width: 100px;
  max-height: 100px;

  @media (max-width: 768px) {
    max-width: 80px;
    max-height: 80px;
  }
`

export const TextWrapper = styled.div`
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
`
