import styled from 'styled-components';
import Button from '../common/Button';

interface PaginationButtonProps {
  noPadding?: boolean;
}

export const PaginationButton = styled(Button)<PaginationButtonProps>`
  border: none;
  padding: ${(props) =>
    props.noPadding ? 0 : `${props.theme.space[3]} ${props.theme.space[2]}`};

  @media screen and (min-width: ${(props) => props.theme.breakPoints.tablet}) {
    padding: ${(props) =>
      props.noPadding ? 0 : `${props.theme.space[3]} ${props.theme.space[3]}`};
  }

  :disabled {
    cursor: default;
  }

  > svg {
    padding-bottom: ${(props) => props.theme.space[1]};
    width: ${(props) => props.theme.fontSizes.s};
    height: ${(props) => props.theme.fontSizes.s};
  }
`;

export const PaginationContainer = styled.div`
  padding-bottom: ${(props) => props.theme.space[4]};
  text-align: center;
  white-space: nowrap;
`;
