import { StyledSpinner, StyledSpinnerWrapper } from './Spinner.styles';

export const Spinner = () => {
  return (
    <StyledSpinnerWrapper data-testid="spinner">
      <StyledSpinner />
    </StyledSpinnerWrapper>
  );
};
