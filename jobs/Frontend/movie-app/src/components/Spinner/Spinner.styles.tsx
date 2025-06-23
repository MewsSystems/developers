import styled from "styled-components";

export const SpinnerWrapper = styled.div`
  display: flex;
  height: 100vh;
  align-items: center;
  justify-content: center;
`;
export const StyledSpinner = styled.div`
  height: 6.5rem;
  width: 6.5rem;
  border: 0.4rem solid;
  border-color: black transparent black transparent;
  border-radius: 50%;
  animation: spin 1.3s linear infinite;
  @keyframes spin {
    to {
      transform: rotate(360deg);
    }
`;
