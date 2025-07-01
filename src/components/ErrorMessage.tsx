import { AiOutlineWarning } from 'react-icons/ai';

export function ErrorMessage({ message }: { message: string }) {
  return (
    <div
      role="alert"
      className="text-red-800 flex items-center gap-2"
      aria-live="polite"
      aria-atomic
    >
      <AiOutlineWarning size={24} />
      <span>{message}</span>
    </div>
  );
}
