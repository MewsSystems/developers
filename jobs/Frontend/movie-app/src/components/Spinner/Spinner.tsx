import { SpinnerWrapper, StyledSpinner } from "./Spinner.styles";

function Spinner() {
  return (
    <SpinnerWrapper data-testid="spinner">
      <StyledSpinner />
    </SpinnerWrapper>
  );
}

export default Spinner;
