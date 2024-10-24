import styled from 'styled-components';
import { ColorsTheme } from '../../assets/colors/theme-colors/colors.ts';

const StyledButton = styled.button`
  cursor: pointer;
  background-color: ${ColorsTheme.secondary};
  border: 1px solid ${ColorsTheme.primary};
  border-radius: 0.5rem;
  height: 2rem;
  width: 7rem;
  padding: 0.5rem 1rem;
`;

export default StyledButton;
