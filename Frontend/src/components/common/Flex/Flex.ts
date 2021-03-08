import styled, { css } from 'styled-components';

const Flex = styled.div<FlexProps>`
  display: ${({ inline }) => (inline ? 'inline-flex' : 'flex')};
  flex-direction: ${({ flexDirection }) => flexDirection || 'row'};
  flex-wrap: ${({ flexWrap }) => flexWrap || 'wrap'};
  justify-content: ${({ justifyContent }) => justifyContent || 'normal'};
  align-content: ${({ alignContent }) => alignContent || 'normal'};
  align-items: ${({ alignItems }) => alignItems || 'normal'};
  gap: ${({ gap }) => gap || 'normal'};

  ${({ full }) =>
    full &&
    css`
      width: 100%;
      height: 100%;
      flex-basis: 100%;
    `}

  ${({ center }) =>
    center &&
    css`
      align-items: center;
      justify-content: center;
    `};
`;

export interface FlexProps {
  flexDirection?: 'row' | 'row-reverse' | 'column' | 'column-reverse';
  flexWrap?: 'wrap' | 'nowrap' | 'wrap-reverse';
  justifyContent?:
    | 'flex-start'
    | 'flex-end'
    | 'center'
    | 'space-between'
    | 'space-around';
  alignContent?:
    | 'flex-start'
    | 'flex-end'
    | 'center'
    | 'strech'
    | 'space-around'
    | 'space-between';
  alignItems?: 'flex-start' | 'flex-end' | 'center' | 'baseline' | 'strech';
  gap?: string;

  inline?: boolean;
  full?: boolean;
  center?: boolean;
}

export default Flex;
