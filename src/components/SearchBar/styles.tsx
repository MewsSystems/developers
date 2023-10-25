import styled from 'styled-components';
import { theme } from '../../styles/theme';

export const Wrapper = styled.div`
  padding: 16px;
  border: 1px solid ${theme.colors.black[300]};
  border-radius: 4px;
`;

export const TextInput = styled.input.attrs({ type: 'text' })``;
