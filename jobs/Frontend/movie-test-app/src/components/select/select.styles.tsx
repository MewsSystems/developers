import styled from 'styled-components';

export const StyledSelect = styled.select`
  background: none;
  background-color: ${(props) => props.theme.secondary};
  color: ${(props) => props.theme.primary};
  font-size: 18px;
  padding: 10px 10px 10px 5px;
  display: block;
  width: 10rem;
  @media screen and (max-width: 800px) {
    width: 8rem;
  }
  @media screen and (max-width: 350px) {
    width: 5rem;
  }
  border: none;
  border-radius: 0;
  border-bottom: 1px solid ${(props) => props.theme.primary};
  margin: 1rem 0;

  &:focus {
    outline: none;
  }
`;

export default StyledSelect;
