import { useNavigate } from "react-router-dom"
import styled from "styled-components"

const NavigationButton = styled.button`
  background-color: transparent;
  color: ${(props) => props.theme.main};
  font-size: 0.8em;
  border: 0;
  padding: 0;
  display: flex;
  justify-content: center;
  .backButtonIcon {
    margin-right: 0.3rem;
  }
  &:hover {
    cursor: pointer;
    .backButtonText {
      text-decoration: underline;
    }
  }
`

function BackNavigationButton() {
  const navigate = useNavigate()

  const handleGoBack = () => {
    const hasPreviousPage = window.history.length > 1

    if (hasPreviousPage) {
      navigate(-1)
    } else {
      navigate("/")
    }
  }
  return (
    <NavigationButton onClick={handleGoBack}>
      <span className="backButtonIcon">{"<"}</span>
      <span className="backButtonText">Return to previous page</span>
    </NavigationButton>
  )
}

export default BackNavigationButton
