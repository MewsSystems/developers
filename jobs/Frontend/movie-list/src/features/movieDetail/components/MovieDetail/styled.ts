import styled from "styled-components";
import { mq } from "~/features/ui/theme/mq";

export const MovieDetailContainer = styled.article`
  display: flex;
  flex-direction: column;
  width: 100%;
  padding: 3rem;
  gap: 2rem;

  ${mq.small} {
    flex-direction: row;
  }
`

export const ImageContainer = styled.div`
  position: relative;

  ${mq.small} {
    flex-basis: 45%;
  }

  ${mq.medium} {
    flex-basis: 35%;
  }

  ${mq.large} {
    flex-basis: 30%;
  }
`;