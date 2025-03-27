import { ErrorReason } from '@/api/base';
import { AlertDescription, AlertTitle } from './ui/Alert';
import { AlertCircle } from 'lucide-react';

interface Props {
  error: ErrorReason | null;
}

const ErrorAlert = ({ error }: Props) => {
  if (error === null) {
    return null;
  }

  const generateErrorMessage = (): string => {
    switch (error) {
      case 'not found': {
        return "We couldn't find what you're looking for. Try searching for a different movie. If the issue persists, please contact support.";
      }
      case 'unauthorized': {
        return "Something went wrong on our end. Please try again later.";
      }
      case 'forbidden': {
        return "Access is restricted. If the issue persists, please contact support.";
      }
      case 'unreachable': {
        return "We're having trouble connecting to the server. Please try again later.";
      }
      case 'unknown': {
        return "An unexpected error occurred. Please try again or contact support.";
      }
      case 'network': {
        return "It looks like you're offline. Check your connection and try again.";
      }
      case 'invalid-response': {
        return "It looks like the received data is invalid. Please try again later or contact support.";
      }
      default: {
        return "Something went wrong. Please refresh the page and try again.";
      }
    }
  };

  return (
    <div className="flex flex-col items-center">
      <div className='flex items-center gap-4'>
        <AlertCircle className="text-destructive h-4 w-4" />
        <AlertTitle>Error</AlertTitle>
      </div>
      <AlertDescription>
        {generateErrorMessage()}
      </AlertDescription>
    </div>
  );
};

export default ErrorAlert;
