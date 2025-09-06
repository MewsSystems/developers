import type { ReactNode } from "react";
import { ErrorMessageContainer, EmptyStateMessage } from "../styles/styles";
import { AlertCircle } from "lucide-react";

type ErrorStateProps = {
  message: string;
  icon?: ReactNode;
};

const ErrorState = ({ message, icon }: ErrorStateProps) => {
  return (
    <ErrorMessageContainer>
      {icon || <AlertCircle size={40} />}
      <EmptyStateMessage>{message}</EmptyStateMessage>
    </ErrorMessageContainer>
  );
};

export default ErrorState;
