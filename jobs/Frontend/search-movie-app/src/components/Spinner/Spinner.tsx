import { SPINNER_TEST_ID } from '../../constants';
import { StyledSpinner, StyledSpinnerWrapper } from './Spinner.styles';

export const Spinner = () => {
  return (
    <StyledSpinnerWrapper data-testid={SPINNER_TEST_ID}>
      <StyledSpinner />
    </StyledSpinnerWrapper>
  );
};
