import styled from "styled-components";

export const Container = styled.div`
  padding: 30px;
  color: ${props => props.theme.textColorPrimary};
  background-color: ${props => props.theme.backgroundColorPrimary};
  position: relative;
  height: 100vh;
  }
`;
