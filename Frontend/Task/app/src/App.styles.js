import styled, { createGlobalStyle } from "styled-components";

export const GlobalStyles = createGlobalStyle`
 *{
  box-sizing: border-box;
 }
  body {
    @import url('https://fonts.googleapis.com/css?family=Lexend+Deca');
    font-family: 'Lexend Deca', sans-serif;
  }
`
export const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    .Container{
      display: flex;
      justify-content: space-evenly;
      width: 100%;
    }
`;