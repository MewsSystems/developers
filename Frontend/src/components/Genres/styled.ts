import styled from 'styled-components';
import Flex from '../common/Flex';

export const GenreContainer = styled(Flex)`
  margin: ${({ theme }) => theme.space[4]} 0;
`;

export const GenreBox = styled.span`
  padding: ${({ theme }) => `${theme.space[2]} ${theme.space[3]}`};
  font-size: ${({ theme }) => theme.fontSizes.m};
  color: ${({ theme }) => theme.colors.text};
  background-color: ${({ theme }) => theme.colors.background};
  border: ${({ theme }) => theme.borders.thin};
  border-color: ${({ theme }) => theme.colors.secondaryDark};
  border-radius: ${({ theme }) => theme.radii.round};
  cursor: default;
`;
