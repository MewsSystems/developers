import styled from 'styled-components';

const StyledButton = styled.button`
  cursor: pointer;
  color: ${(props) => props.theme.colors.primary};
  background-color: ${(props) => props.theme.colors.secondary};
  border: 1px solid ${(props) => props.theme.colors.primary};
  border-radius: 0.5rem;
  height: 2rem;
  width: 7rem;
  padding: 0.5rem 1rem;
`;

export default StyledButton;
