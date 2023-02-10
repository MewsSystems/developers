import styled from "styled-components";

export const Detail = styled.div`
  display: flex;
  justify-content: center;
  margin-top: 20px;
`;
export const Overview = styled.p`
  margin-top: 20px;
`;
export const Scores = styled.div`
  margin-top: 30px;
  display: flex;

  align-items: center;
`;

export const SubTitle = styled.p`
  margin-top: 15px;
  font-size: 18px;
  color: ${props => props.theme.textColorSecondary};
`;
