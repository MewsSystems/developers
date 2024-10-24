import styled from 'styled-components';

const StyledButton = styled.button`
  cursor: pointer;
  color: ${(props) => props.theme.primary};
  background-color: ${(props) => props.theme.secondary};
  border: 1px solid ${(props) => props.theme.primary};
  border-radius: 0.5rem;
  height: 2rem;
  width: 7rem;
  padding: 0.5rem 1rem;
`;

export default StyledButton;
