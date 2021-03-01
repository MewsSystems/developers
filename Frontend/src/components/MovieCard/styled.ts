import styled from 'styled-components';

export interface CardProps {
  width?: string;
  height: string;
}

export const Card = styled.div<CardProps>`
  display: flex;
  border: 1px solid ${(props) => props.theme.colors.secondaryDark};
  box-shadow: 2px 2px 2px ${(props) => props.theme.colors.secondaryDark};
  width: ${(props) => props.width || 'auto'};
  height: ${(props) => props.height};
  color: ${(props) => props.theme.colors.text};
  background-color: ${(props) => props.theme.colors.secondaryLight};
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

export const CardTitle = styled.h3`
  margin: 0 0 0.3em 0;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;

  > small {
    font-style: italic;
    color: ${(props) => props.theme.colors.secondaryDark};
  }
`;

export const CardText = styled.div`
  overflow: hidden;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
`;

export const CardsList = styled.div`
  padding-top: ${(props) => props.theme.space[5]};
  padding-bottom: ${(props) => props.theme.space[5]};

  ${Card} + ${Card} {
    margin-top: 1em;
  }
`;
