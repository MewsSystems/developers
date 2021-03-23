import styled from 'styled-components';
import Container from '../common/Container';
import { Title } from '../common/Title';

export const MovieDetailContainer = styled(Container)`
  strong + p {
    margin-top: ${({ theme }) => theme.space[3]};
  }
`;

export const MovieDetailTitle = styled(Title).attrs({
  as: 'h1',
})`
  margin-top: 0;
  margin-bottom: ${({ theme }) => theme.space[4]};
`;
