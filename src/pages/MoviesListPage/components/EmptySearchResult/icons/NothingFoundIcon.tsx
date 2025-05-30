import {Icon} from '../NothingFoundState.styled.tsx';

export default function NothingFoundIcon() {
  return (
    <Icon
      width="120"
      height="120"
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="1.5"
      strokeLinecap="round"
      strokeLinejoin="round"
    >
      <circle cx="12" cy="12" r="10" />
      <path d="M8 9.05v-.1" />
      <path d="M16 9.05v-.1" />
      <path d="M16 16c-.5-1.5-1.79-3-4-3s-3.5 1.5-4 3" />
    </Icon>
  );
}
