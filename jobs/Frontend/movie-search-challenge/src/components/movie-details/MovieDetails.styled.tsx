import styled from "styled-components"

export const StyledMovieDetails = styled.div`
  display: flex;
  flex-direction: row;
  color: white;
  max-width: 800px;
  background-color: ${({ theme }) => theme.colors.secondary};
  margin-top: 40px;

  > div:first-child {
    display: flex;
    align-items: center;

    img {
      margin: 20px;
      width: 300px;
      height: 400px;
      border-radius: 5px;
    }
  }

  > div:last-child {
    padding: 10px;
  }

  h1,
  h2,
  h3 {
    margin: 10px 0;
  }

  @media (max-width: ${({ theme }) => theme.mobile}) {
    flex-direction: column;
    align-items: center;
    margin-top: 0;
  }
`
