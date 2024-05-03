import styled from "styled-components";

const Spinner = styled.div`
  position: fixed;
  top: 50%;
  left: 50%;  
  width: 88px;
  height: 88px;  
  margin-top: -44px;
  margin-left: -44px;
  border: 8px solid rgba(230, 230, 230, 0.88);
  border-radius: 50%;
  border-top-color: black;
  animation: spin 1s ease-in-out infinite;

  @keyframes spin {
    to {
      transform: rotate(360deg);
    }
  }
`;

export default Spinner;
