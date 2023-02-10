import styled from "styled-components";

export const Item = styled.li`
  border-bottom: 1px solid ${props => props.theme.textColorSecondary};
  width: 90%;
  display: flex;
  align-items: center;
  margin-bottom: 10px;
  padding: 15px 10px;
  height: 100px;
`;

export const Row = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
`;
export const RatingsStyled = styled.div`
  display: flex;
  align-items: center;
`;

export const MovieInfo = styled.div`
  display: flex;
  flex-direction: column;
  a {
    text-decoration: none;
  }
`;

export const MovieName = styled.h3`
  margin: 0;
  font-weight: 500;
  font-size: 20px;
  line-height: 24px;
  color: ${props => props.theme.textColorPrimary};
  margin-bottom: 5px;
`;

export const Release = styled.span`
  margin: 0;
  font-weight: 400;
  font-size: 14px;
  color: ${props => props.theme.textColorSecondary};
`;
