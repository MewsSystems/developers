import styled from 'styled-components'

export const Wrapper = styled.div`
  transition: filter 0.1s;
  margin-bottom: 48px;

  @media (max-width: 1050px) {
    margin-bottom: 0;
  }

  &:hover {
    filter: grayscale(100%);
    transition: filter 0.4s;
  }
`

export const Title = styled.div`
  max-width: 342px;
  margin-bottom: 32px;

  @media (max-width: 1050px) {
    margin-bottom: 16px;
  }
`

export const VoteAverage = styled.span`
  position: relative;
  top: -20px;
  left: 8px;
`

export const Poster = styled.div`
  position: relative;
  max-width: 396px;
`

export const ReleaseDate = styled.div`
  position: absolute;
  right: 0;
  top: 0;
  writing-mode: tb-rl;
  transform: rotate(-180deg);

  @media (max-width: 1050px) {
    position: unset;
    writing-mode: unset;
    transform: unset;
    margin-bottom: 24px;
  }
`
