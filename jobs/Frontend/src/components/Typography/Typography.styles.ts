import { theme } from '@/theme'
import styled, { css } from 'styled-components'

export type TypographyVariants = keyof typeof typographyVariants
type StyledTypes = {
  $color: 'primary' | 'secondary'
  $userSelect: boolean
}

const sharedStyles = css<StyledTypes>`
  display: inline-flex;
  color: ${(props) =>
    props.$color === 'primary'
      ? theme.colors.text.primary
      : theme.colors.text.secondary};
  user-select: text;

  ${({ $userSelect }) =>
    !$userSelect &&
    css`
      user-select: none;
    `}
`

export const typographyVariants = {
  primaryHeading: styled.h1<StyledTypes>`
    ${sharedStyles}
    font-size: ${theme.sizes.primaryHeading};
    letter-spacing: -0.5rem;
    margin-top: -48px;
    margin-left: -20px;

    @media (max-width: 1920px) {
      margin-top: 0;
      font-size: 10rem;
    }
  `,
  secondaryHeading: styled.h2<StyledTypes>`
    ${sharedStyles}
    font-size: ${theme.sizes.secondaryHeading};
  `,
  primarySpan: styled.span<StyledTypes>`
    ${sharedStyles}
    font-size: ${theme.sizes.primarySpan};
  `,
  secondarySpan: styled.span<StyledTypes>`
    ${sharedStyles}
    font-size: ${theme.sizes.secondarySpan};
  `,
  tertiarySpan: styled.span<StyledTypes>`
    ${sharedStyles}
    font-size: ${theme.sizes.tertiarySpan};
  `,
}
