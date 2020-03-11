import styled from 'styled-components';

const Button = styled.button`
  width: 64px;
  height: 48px;
  font-size: 24px;
  border: none;
  margin: 1px;
  border-color: ${props => props.theme.border.primary};
  color: ${props => (props.disabled ? props.theme.text.disabled : props.theme.text.primary)};
  background-color: ${props => (props.disabled ? props.theme.background.primary : props.theme.background.withContent)};
  cursor: ${props => (props.disabled ? 'not-allowed' : 'pointer')};
  &:hover {
    background-color: ${props => props.theme.background.hover};
  }
`;

export default Button;
