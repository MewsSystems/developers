import Image from 'next/image'
import styled from 'styled-components'

type StyledProps = {
  $isColorless: boolean
}

export const StyledLogo = styled(Image)<StyledProps>`
  filter: ${(props) =>
    props.$isColorless ? 'brightness(0) invert(1)' : 'none'};

  @media (max-width: 768px) {
    width: 150px;
    height: 20px;
  }
`
