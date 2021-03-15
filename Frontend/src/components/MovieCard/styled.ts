import styled from 'styled-components';
import { Title } from '../common/Title';

export interface CardProps {
  width?: string;
  height: string;
}

export const Card = styled.div<CardProps>`
  display: flex;
  border: 1px solid ${({ theme }) => theme.colors.secondaryDark};
  box-shadow: 2px 2px 2px ${({ theme }) => theme.colors.secondaryDark};
  width: ${({ width }) => width || 'auto'};
  height: ${({ height }) => height};
  color: ${({ theme }) => theme.colors.text};
  background-color: ${({ theme }) => theme.colors.secondaryLight};
  text-decoration: none;

  :hover {
    box-shadow: none;
  }

  & > * {
    flex-shrink: 0;
  }
`;

export const CardBody = styled.div`
  flex: 1;
  padding: 10px;
  overflow: hidden;

  > * {
    margin-top: 0.5em;
  }
`;

export const CardTitle = styled(Title).attrs({
  as: 'h3',
})`
  margin: 0 0 0.3em 0;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
`;

export const CardText = styled.div`
  overflow: hidden;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
`;

export const CardsList = styled.div`
  padding-top: ${({ theme }) => theme.space[5]};
  padding-bottom: ${({ theme }) => theme.space[5]};

  ${Card} + ${Card} {
    margin-top: 1em;
  }
`;
