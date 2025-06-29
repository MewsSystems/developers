import styled, { type CSSProperties } from "styled-components";

interface StyledImageProps {
  $width: CSSProperties["width"];
}

export const StyledImage = styled.img<StyledImageProps>`
  width: ${(props) => (props.$width ? props.$width : "100%")};
  height: 100%;
  object-fit: cover;
  transition: all 0.3s;
`;
