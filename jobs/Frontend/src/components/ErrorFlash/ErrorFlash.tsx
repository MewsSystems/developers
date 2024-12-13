import { useContext } from "react";
import {
  ErrorContainer,
  ErrorMessage,
  CloseButton,
} from "@/components/ErrorFlash/ErrorFlashStyle";
import ErrorContext from "@/providers/ErrorContext";

const ErrorFlash: React.FC = () => {
  const { message, setMessage } = useContext(ErrorContext);

  return (
    message && (
      <ErrorContainer>
        <ErrorMessage>{message}</ErrorMessage>
        <CloseButton onClick={() => setMessage("")}>&times;</CloseButton>
      </ErrorContainer>
    )
  );
};

export default ErrorFlash;
