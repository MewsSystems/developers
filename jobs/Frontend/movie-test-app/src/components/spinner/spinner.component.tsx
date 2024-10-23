import { SpinnerContainer, SpinnerOverlay } from './spinner.styles.js';

const Spinner = () => (
  <SpinnerOverlay data-testid="spinner">
    <SpinnerContainer />
  </SpinnerOverlay>
);

export default Spinner;
