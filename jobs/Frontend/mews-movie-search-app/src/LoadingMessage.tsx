/* Global imports */
import * as React from "react";
import styled from "styled-components";

/* Local imports */

/* Types  */

/* Local utility functions */

/* Component definition */
export const LoadingMessage = ({ text }: { text?: string }) => {
  return (
    <MessageContainer>
      <span>{text ?? "Loading movies ..."}</span>
    </MessageContainer>
  );
};

const MessageContainer = styled.div`
  padding: 1rem;
  height: 100vh;
  width: 100%;
  display: flex;
  justify-content: center;
  align-items: center;

  & > span {
    font-size: 2rem;
    color: #e3fef7;
  }
`;
