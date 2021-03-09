import styled from 'styled-components';
import Flex from '../common/Flex';

export const EmptyStateContainer = styled(Flex).attrs({
  full: true,
  center: true,
  flexDirection: 'column',
})`
  text-align: center;
`;

export const EmptyStateTitle = styled.div`
  font-family: ${({ theme }) => theme.fonts.heading};
  font-weight: ${({ theme }) => theme.fontWeights.bold};
  font-size: ${({ theme }) => theme.fontSizes.l};
  line-height: ${({ theme }) => theme.lineHeights.heading};
  padding-top: ${({ theme }) => theme.space[3]};
  padding-bottom: ${({ theme }) => theme.space[3]};
`;

export const EmptyStateBody = styled.div`
  padding: 0;
`;
