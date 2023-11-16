import styled from "styled-components";

export const H1 = styled.h1<{$fontSize?: string}>`
  font-size: ${props => props.$fontSize}
`

export const H2 = styled.h2<{$fontSize?: string}>`
  font-size: ${props => props.$fontSize}
`

export const H3 = styled.h3<{$fontSize?: string; $fontWeight?: string}>`
  font-size: ${props => props.$fontSize};
  font-weight: ${props => props.$fontWeight};
`

export const H4 = styled.h4<{$fontSize?: string; $fontWeight?: string}>`
  font-size: ${props => props.$fontSize};
  font-weight: ${props => props.$fontWeight};
  margin-block-end: 0;
  margin-block-start: 0;
`

export const Section = styled.section`
  text-align: left
`
