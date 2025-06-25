import { AlertTriangle } from "lucide-react"
import { ErrorContainer, ErrorIcon, ErrorText } from "./ErrorMessage.styles"

export interface ErrorMessageProps {
  message: string
}

export const ErrorMessage = ({ message }: ErrorMessageProps) => {
  return (
    <ErrorContainer>
      <ErrorIcon>
        <AlertTriangle size={24} />
      </ErrorIcon>
      <ErrorText>{message}</ErrorText>
    </ErrorContainer>
  )
}
