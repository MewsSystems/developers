import styled from "styled-components";

export const Img = styled.img<{$width: string, $alignSelf?:string }>`
  width: ${(props) => props.$width};
  align-self: ${(props) => props.$alignSelf};
`;
