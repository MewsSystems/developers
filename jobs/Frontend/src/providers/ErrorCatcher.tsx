import { useState } from "react";
import ErrorContext from "@/providers/ErrorContext";

interface ErrorProps {
  children: React.ReactNode;
}
const ErrorCatcher: React.FC<ErrorProps> = ({ children }) => {
  const [message, setMessage] = useState("");

  return (
    <ErrorContext.Provider value={{ message, setMessage }}>
      {children}
    </ErrorContext.Provider>
  );
};

export default ErrorCatcher;
