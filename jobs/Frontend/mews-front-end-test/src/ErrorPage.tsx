import { useRouteError } from 'react-router-dom';
import { FC } from 'react';

interface ErrorWithStatus {
  statusText: string;
}

interface ErrorWithMessage {
  message: string;
}

const isErrorWithStatus = (error: unknown): error is ErrorWithStatus =>
  Boolean((error as ErrorWithStatus).statusText);

const isErrorWithMessage = (error: unknown): error is ErrorWithMessage =>
  Boolean((error as ErrorWithMessage).message);

const ErrorPage: FC = () => {
  const error = useRouteError();
  console.error(error);

  const getMessage = (error: unknown) => {
    if (isErrorWithStatus(error)) {
      return error.statusText;
    }

    if (isErrorWithMessage(error)) {
      return error.message;
    }
  };

  return (
    <div>
      <h1>Oops!</h1>
      <p>Sorry, an unexpected error has occurred.</p>
      <p>
        <i>{getMessage(error)}</i>
      </p>
    </div>
  );
};

export { ErrorPage };
