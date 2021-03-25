import { FallbackProps } from 'react-error-boundary';
import { useHistory } from 'react-router';
import Button from './common/Button';
import { ErrorState } from './EmptyState';

function ErrorFallback({ error, resetErrorBoundary }: FallbackProps) {
  const history = useHistory();
  return (
    <ErrorState title="Oops.. Something went wrong!">
      <pre>{error.message}</pre>
      <Button
        onClick={() => {
          history.push('/');
          resetErrorBoundary();
        }}
      >
        Try again
      </Button>
    </ErrorState>
  );
}

export default ErrorFallback;
