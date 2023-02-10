import styled from "styled-components";

export const Button = styled.button`
  border: 1px solid transparent;
  background-color: transparent;
  padding: 10px 15px;
  margin: 0 5px;
  cursor: pointer;

  color: ${props => props.theme.textColorSecondary};

  &:hover {
    color: ${props => props.theme.textColorPrimary};
    border-bottom: 1px solid ${props => props.theme.textColorPrimary};
  }

  &.active {
    color: ${props => props.theme.textColorPrimary};
    border-bottom: 3px solid ${props => props.theme.highlighted};
  }
`;
