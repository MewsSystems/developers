import { AiOutlineLoading3Quarters } from 'react-icons/ai';

export function LoadingIndicator() {
  return (
    <span role="status" aria-live="polite" className="absolute right-3 animate-spin text-cyan-950">
      <AiOutlineLoading3Quarters aria-hidden="true" className="w-4 h-4" />
      <span className="sr-only">Loading...</span>
    </span>
  );
}
