import styled from "styled-components";

export const TagStyled = styled.span`
  color: ${props => props.theme.textColorPrimary};
  padding: 5px 10px;
  border: 1px solid ${props => props.theme.textColorPrimary};
  margin-right: 10px;
  border-radius: 20px;
`;
