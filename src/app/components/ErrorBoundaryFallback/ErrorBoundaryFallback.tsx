import {useNavigate} from 'react-router-dom';
import type {FallbackProps} from 'react-error-boundary';
import {
  Button,
  ButtonGroup,
  ErrorContainer,
  ErrorMessage,
  ErrorTitle,
} from './ErrorBoundaryFallback.styled';
import {PageType, PathByPageType} from '../../../routes/constants.ts';

export default function ErrorBoundaryFallback({error, resetErrorBoundary}: FallbackProps) {
  const navigate = useNavigate();

  const onReset = () => {
    resetErrorBoundary();
    navigate(PathByPageType[PageType.MoviesListPage]);
  };

  return (
    <ErrorContainer>
      <ErrorTitle>Oops! Something went wrong</ErrorTitle>
      <ErrorMessage>
        {error.message || 'An unexpected error occurred. Please try again later.'}
      </ErrorMessage>
      <ButtonGroup>
        <Button onClick={onReset}>Go to Homepage</Button>
        <Button onClick={() => window.location.reload()}>Reload Page</Button>
      </ButtonGroup>
    </ErrorContainer>
  );
}
