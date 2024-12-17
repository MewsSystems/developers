import { createContext, Dispatch, SetStateAction } from "react";

interface IErrorContext {
  message: string;
  setMessage: Dispatch<SetStateAction<string>>;
}

const ErrorContext = createContext<IErrorContext>({
  message: "",
  setMessage: () => {},
});

export default ErrorContext;
