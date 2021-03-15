import styled from 'styled-components';
import Flex, { FlexItem } from '../common/Flex';

export const PageHeader = styled(Flex).attrs({
  as: 'header',
  flexWrap: 'nowrap',
  center: true,
})`
  padding: ${({ theme }) => theme.space[4]};
  height: ${({ theme }) => theme.space[5]};
  line-height: ${({ theme }) => theme.lineHeights.heading};
  font-size: ${({ theme }) => theme.fontSizes.l};
  background-color: ${({ theme }) => theme.colors.primary};
  color: ${({ theme }) => theme.colors.secondaryLight};
`;

export const PageMain = styled(FlexItem).attrs({
  as: 'main',
  $display: 'flex',
  grow: 1,
})`
  background-color: ${({ theme }) => theme.colors.background};
`;

export const PageFooter = styled(Flex).attrs({
  as: 'footer',
})`
  padding: ${({ theme }) => theme.space[4]};
`;
