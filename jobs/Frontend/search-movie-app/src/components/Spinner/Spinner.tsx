import { StyledSpinner, StyledSpinnerWrapper } from './Spinner.styles';

const Spinner = () => {
  return (
    <StyledSpinnerWrapper data-testid="spinner">
      <StyledSpinner />
    </StyledSpinnerWrapper>
  );
};

export { Spinner };
