import { styled } from 'styled-components';
import { theme } from '../../styles/theme';

export const NoResultsWrapper = styled.div`
  margin-top: ${theme.spacing.md}px;
  background-color: ${theme.colors.white[100]};
  padding: ${theme.spacing.md}px;
  border-radius: 4px;
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100px;
  text-align: center;
`;
