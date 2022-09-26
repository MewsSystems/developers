import styled from "styled-components";
import { palette } from "~/features/ui/theme/palette";

export const MovieCard = styled.article`
  height: fit-content;
  overflow: hidden;
  transition: 300ms;
  &:hover {
    transform: scale(1.1);
  }
`;


export const MovieCardImage = styled.div`
  position: relative;
  cursor: pointer;
  transition: 0.3s;
  overflow: hidden;
`

export const MovieCardDetails = styled.div`
  padding: 5px;
`

export const MovieCardTitle = styled.h2`
  font-size: 1.6rem;
  color: ${palette.black};

  &:hover {
    color: ${palette.blue[100]};
  }
`

export const MovieCardDate = styled.p`
  font-size: 1.2rem;
`