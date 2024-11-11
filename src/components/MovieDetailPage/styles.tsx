import { styled } from 'styled-components';
import { theme } from '../../styles/theme';

export const DetailWrapper = styled.div<{ backdrop?: string }>`
  margin-top: ${theme.spacing.md}px;
  background: ${theme.colors.black[100]};
  display: flex;
  background: ${({ backdrop }) => backdrop && `url(${backdrop})`} no-repeat top;
  background-size: cover;
  height: 100%;
  border: 1px solid ${theme.colors.white[300]};
  border-radius: 4px;
  padding: ${theme.spacing.md}px;
  flex-direction: column;
  color: ${theme.colors.white[300]};
`;

export const Body = styled.div`
  display: flex;

  @media screen and (max-width: 480px) {
    flex-direction: column;
    align-items: center;
  }
`;

export const Poster = styled.div`
  border: 2px solid ${theme.colors.white[300]};
  flex: 1;
  margin-right: ${theme.spacing.md}px;
  overflow: hidden;
  border-radius: 4px;
  img {
    max-width: 100%;
    max-height: 100%;
  }

  @media screen and (max-width: 480px) {
    flex: 1;
    margin: auto;
    margin-bottom: ${theme.spacing.md}px;
  }
`;

export const Overview = styled.div``;

export const Details = styled.div`
  border: 2px solid ${theme.colors.white[300]};
  background-color: ${theme.colors.black[300]};
  padding: ${theme.spacing.md}px;
  overflow: hidden;
  border-radius: 4px;
  opacity: 0.8;
  flex: 3;
  display: flex;
  flex-direction: column;
  justify-content: space-between;

  @media screen and (max-width: 480px) {
    flex: 1;
  }
`;

export const InfoItem = styled.span`
  font-size: 11px;
  flex: 1 1 100px;
  line-height: 1.5;
`;

export const Info = styled.div`
  display: flex;
  flex-wrap: wrap;
`;

export const BackLink = styled.div`
  background-color: ${theme.colors.black[300]};
  padding: ${theme.spacing.md}px;
  margin-top: ${theme.spacing.md}px;
  border: 2px solid ${theme.colors.white[300]};
  overflow: hidden;
  border-radius: 4px;

  > * {
    color: ${theme.colors.white[300]};
    text-decoration: none;
  }
`;
