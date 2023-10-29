import styled from 'styled-components';
import { theme } from '../../styles/theme';
import { Link } from 'react-router-dom';

export const ListContainer = styled.div`
  display: flex;
  justify-content: space-between;
  flex-wrap: wrap;

  @media screen and (max-width: 480px) {
    flex-direction: column;
    align-items: center;
  }
`;

export const Card = styled.div`
  position: relative;
  overflow: hidden;
  border-radius: 4px;
  margin-top: ${theme.spacing.md}px;
  width: 320px;
  height: 480px;
  background-color: ${theme.colors.black[100]};
  text-decoration: none;
  color: ${theme.colors.black[300]};
  display: flex;
  justify-content: center;
  flex-direction: column;
  cursor: pointer;

  @media screen and (max-width: 480px) {
    width: 100%;
    height: unset;
  }
`;

export const Info = styled.div<{ isSelected?: boolean }>`
  position: absolute;
  display: flex;
  left: 0;
  top: 0;
  flex-direction: column;
  padding: ${theme.spacing.md}px;
  width: 100%;
  flex: 1;
  z-index: 99;
  background-color: ${theme.colors.white[300]};
  opacity: ${({ isSelected }) => (isSelected ? 0.8 : 0)};
  height: 100%;
  transition: all 100ms ease-in;

  @media screen and (min-width: 480px) {
    &:hover {
      opacity: 0.8;
    }
  }
`;

export const DetailsLink = styled.div`
  text-transform: uppercase;
  flex: 1;
  text-decoration: none;
  > * {
    color: ${theme.colors.red[300]};
  }
`;

export const PosterWrapper = styled.div`
  display: flex;
  justify-content: center;
`;

export const Poster = styled.img`
  max-width: 100%;
`;

export const Title = styled.div`
  margin-top: 0;
  font-size: 36px;
  font-weight: bolder;
  flex: 1;
`;

export const Overview = styled.div`
  overflow: scroll;
  padding: ${theme.spacing.md}px 0;
  flex: 3;
`;

export const Stats = styled.div`
  display: flex;
  justify-content: space-between;
  flex: 1;
`;

export const StatItem = styled.span`
  font-size: 10px;
  opacity: 0.5;
`;
