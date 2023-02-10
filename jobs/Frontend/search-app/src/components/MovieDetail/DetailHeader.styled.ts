import styled from "styled-components";

export const Header = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid ${props => props.theme.textColorSecondary};
`;

export const Title = styled.div`
  color: ${props => props.theme.textColorPrimary};
  padding-bottom: 10px;
`;

export const Back = styled.i`
  font-size: 30px;
  margin-right: 20px;
  color: ${props => props.theme.textColorSecondary};
  cursor: pointer;
  :hover {
    color: ${props => props.theme.textColorPrimary};
  }
`;
