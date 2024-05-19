import { useNavigate } from "react-router-dom";
import { BackButtonContainer, StyledBackButton } from "./BackButton.styles";

export function BackButton() {
  let navigate = useNavigate();

  return (
    <BackButtonContainer>
      <StyledBackButton
        onClick={() => navigate(-1)}
      >{`< Back`}</StyledBackButton>
    </BackButtonContainer>
  );
}

export default BackButton;
