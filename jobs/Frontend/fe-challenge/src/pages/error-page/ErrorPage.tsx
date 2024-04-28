import { useRouteError } from 'react-router-dom';

const ErrorPage = () => {
  const error = useRouteError() as Error;

  return (
    <section>
      <h1>Uh oh, something went terribly wrong 😩</h1>
      <pre className="whitespace-pre-wrap mb-4">
        {error.message || JSON.stringify(error)}
      </pre>
      <button onClick={() => (window.location.href = '/')}>
        Click here to reload the app
      </button>
    </section>
  );
};

export default ErrorPage;
