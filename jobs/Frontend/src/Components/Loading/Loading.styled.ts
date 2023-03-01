import styled from "styled-components";

export const LoadingElement = styled.div`
  @keyframes pulse {
    0% { opacity: 0.3; }
    50% { opacity: 1; }
    100% { opacity: 0.3; }
  }

  & > div {
    animation: pulse 0.5s infinite ease-in-out;
  }
`;
