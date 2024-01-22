import styled from "styled-components"

export const StyledMovieDetails = styled.div`
  display: flex;
  flex-direction: row;
  color: white;
  max-width: 800px;
  background-color: ${({ theme }) => theme.colors.secondary};

  > div:first-child {
    img {
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
  }
`
