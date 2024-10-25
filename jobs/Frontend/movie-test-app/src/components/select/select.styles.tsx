import styled from 'styled-components';

const StyledSelect = styled.select`
  background: none;
  background-color: ${(props) => props.theme.colors.secondary};
  color: ${(props) => props.theme.colors.primary};
  font-size: 18px;
  padding: 10px 10px 10px 5px;
  display: block;
  width: 10rem;
  @media screen and (max-width: ${(props) => props.theme.breakpoints.mobile}) {
    width: 7rem;
  }
  @media screen and (max-width: ${(props) => props.theme.breakpoints.smallMobile}) {
    width: 5rem;
  }
  border: none;
  border-radius: 0;
  border-bottom: 1px solid ${(props) => props.theme.colors.primary};
  margin: 1rem 0;

  &:focus {
    outline: none;
  }
`;

const StyledLabel = styled.label`
  color: ${(props) => props.theme.colors.primary};
  font-size: 12px;
  font-weight: normal;
  position: absolute;
  pointer-events: none;
  left: 0.1rem;
  transition: 300ms ease all;
  top: 0.5rem;
  color: ${(props) => props.theme.colors.primary};
`;

const Group = styled.div`
  position: relative;
`;

export { StyledSelect, Group, StyledLabel };
