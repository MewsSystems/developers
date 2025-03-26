import { ErrorReason } from '@/api/base';

interface Props {
  error: ErrorReason | null;
}

const ErrorAlert = ({ error }: Props) => {
  if (error === null) {
    return null;
  }

  const renderError = () => {
    switch (error) {
      case 'not found': {
        return <div>We couldn't find what you're looking for. Try searching for a different movie. If the issue persists, please reach out.</div>;
      }
      case 'unauthorized': {
        return <div>Oops! Something went wrong on our end. Please try again later.</div>;
      }
      case 'forbidden': {
        return <div>Access is restricted. If the issue persists, please contact support.</div>;
      }
      case 'unreachable': {
        return <div>We're having trouble connecting to the server. Please try again later.</div>;
      }
      case 'unknown': {
        return <div>An unexpected error occurred. Please try again or contact support.</div>;
      }
      case 'network': {
        return <div>It looks like you're offline. Check your connection and try again.</div>;
      }
      case 'invalid-response': {
        return <div>It looks like the received data is invalid. Please try again later or contact support.</div>;
      }
      default: {
        return <div>Something went wrong. Please refresh the page and try again.</div>;
      }
    }
  };

  return (
    <div className="flex justify-center items-center w-full h-full">
      {renderError()}
    </div>
  );

};

export default ErrorAlert;
