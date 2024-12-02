import { ErrorWrapper } from "./Error.styles";

function Error({ errorMessage }: { errorMessage?: string }) {
  return (
    <ErrorWrapper>
      <h2> An error has occurred 😬, please try again!</h2>
      <i>{errorMessage}</i>
    </ErrorWrapper>
  );
}

export default Error;
