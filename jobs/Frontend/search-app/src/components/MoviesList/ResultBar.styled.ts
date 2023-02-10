import styled from "styled-components";

export const Bar = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 15px 30px;
  border-bottom: 1px solid ${props => props.theme.textColorSecondary};
  background-color: ${props => props.theme.backgroundColorPrimary};
`;

export const Total = styled.h2`
  color: ${props => props.theme.textColorPrimary};
`;
