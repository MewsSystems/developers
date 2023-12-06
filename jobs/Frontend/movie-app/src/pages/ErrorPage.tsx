import { useRouteError, isRouteErrorResponse } from "react-router-dom"
import styled from "styled-components"

const ErrorPageContainer = styled.div`
  display: flex;
  height: 100vh;
  align-items: center;
  justify-content: center;
  flex-direction: column;
  & > h1,
  p {
    text-align: center;
    margin: 1rem 0;
    i {
      color: ${(props) => props.theme.gray};
    }
  }
`

export default function ErrorPage() {
  const error = useRouteError()
  let errorMessage: string
  if (isRouteErrorResponse(error)) {
    errorMessage = error.statusText || error.data?.message
  } else {
    errorMessage = "Unknown error"
  }

  return (
    <ErrorPageContainer id="error-page">
      <h1>Oops!</h1>
      <p>Sorry, an unexpected error has occurred.</p>
      <p>
        <i>{errorMessage}</i>
      </p>
    </ErrorPageContainer>
  )
}
