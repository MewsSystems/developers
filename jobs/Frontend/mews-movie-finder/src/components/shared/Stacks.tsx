import styled from "styled-components";

export const VStack = styled.div<{
  $alignItems?: string;
  $justifyContent?: string;
  $width?: string;
  $textAlign?: string;
}>`
  display: flex;
  flex-direction: column;
  align-items: ${(props) => props.$alignItems};
  justify-content: ${(props) => props.$justifyContent};
  width: ${(props) => props.$width};
  text-align: ${(props) => props.$textAlign};
`;

export const HStack = styled.div<{
  $alignItems?: string;
  $justifyContent?: string;
  $width?: string;
}>`
  display: flex;
  flex-direction: row;
  align-items: ${(props) => props.$alignItems};
  justify-content: ${(props) => props.$justifyContent};
  width: ${(props) => props.$width};
`;
