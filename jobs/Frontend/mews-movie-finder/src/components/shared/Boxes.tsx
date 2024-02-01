import styled from "styled-components";

export const ShadowBox = styled.div<{ $width?: string; $marginRight?: string; }>`
  display: flex;
  background-color: rgb(26, 33, 48);
  padding: 12px;
  border-radius: 8px;
  box-shadow: rgba(0, 0, 0, 0.25) 4px 4px 8px, rgb(41, 51, 66) -2px -2px 6px;
  width: ${props => props.$width};
  margin-right: ${props => props.$marginRight};
`;