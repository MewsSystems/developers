import styled from "styled-components";
import { SpinnerCircle } from "../Spinner/Spinner.internal";

export const GridWrapper = styled.div`
  width: 100%;
  height: auto;
  display: flex;
  justify-content: flex-start;
  gap: 2rem;
`;

export const StyledGrid = styled.div`
  max-width: 100%;
  width: 100%;
  margin: 0 auto;
  padding: 1.8rem 0;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(10rem, 1fr));
  align-items: stretch;
  justify-content: center;
  gap: 1.2rem;
`;

interface StyledGridCardProps {
  $isHovered: boolean;
}

export const StyledGridCard = styled.div<StyledGridCardProps>`
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-start;
  border: none;
  overflow: hidden;

  transition: all 0.3s ease-out;

  transform: ${(props) => (props.$isHovered ? "scale(1.35)" : "scale(1)")};
  z-index: ${(props) => (props.$isHovered ? "50" : "1")};
  box-shadow: ${(props) =>
    props.$isHovered ? "0 25px 50px -12px rgba(0, 0, 0, 0.25)" : "none"};
`;

export interface GridStatusMessageProps {
  isError: boolean;
  isPending: boolean;
  isSuccess: boolean;
  isNoResults: boolean;
  noResultsText?: string;
  pendingText?: string;
  errorText?: string;
  children?: React.ReactNode;
}

const GridStatusMessageContainer = styled.div`
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: column;
  padding: 2rem;
  color: #333;
  font-size: 1rem;
  gap: 0.5rem;
`;

export const Text = styled.p`
  color: #333;
  font-weight: 800;
`;

const ErrorText = styled.p`
  color: #c0392b;
  font-weight: 800;
`;

export const GridStatusMessage = (props: GridStatusMessageProps) => {
  if (props.isNoResults) {
    return (
      <GridStatusMessageContainer>
        <SpinnerCircle />
        <span>
          {props.noResultsText === undefined
            ? "No more results found"
            : props.pendingText}
        </span>
      </GridStatusMessageContainer>
    );
  }

  if (props.isPending) {
    return (
      <GridStatusMessageContainer>
        <SpinnerCircle />
        <span>
          {props.pendingText === undefined ? "Loading…" : props.pendingText}
        </span>
      </GridStatusMessageContainer>
    );
  }

  if (props.isError) {
    return (
      <GridStatusMessageContainer>
        <ErrorText>
          {props.errorText === undefined
            ? "Something went wrong."
            : props.errorText}
        </ErrorText>
      </GridStatusMessageContainer>
    );
  }

  if (props.isSuccess) {
    return <>{props.children}</>;
  }

  return null;
};

export const GridFooterMessageWrapper = styled.div`
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  align-self: flex-end;
`;
