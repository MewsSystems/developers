import styled from "styled-components";

export const RatingStyled = styled.div`
  color: ${props => props.theme.textColorPrimary};
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  margin-right: 30px;

  div:first-child {
    font-size: 14px;
    color: ${props => props.theme.textColorSecondary};
    margin-bottom: 5px;
  }

  div:nth-child(2) {
    display: flex;
    justify-content: center;
    align-items: center;
  }

  i {
    margin-right: 5px;
  }

  .highlighted {
    color: ${props => props.theme.highlighted};
  }
`;
