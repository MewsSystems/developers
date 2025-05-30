import {Icon} from '../EmptyInitialState.styled';

export default function EmptyStateIcon() {
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
      <path d="M4 4h16a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2z" />
      <path d="M22 8H2" />
      <path d="M7 4L5 8" />
      <path d="M11 4L9 8" />
      <path d="M15 4L13 8" />
      <path d="M19 4L17 8" />
    </Icon>
  );
}

EmptyStateIcon.displayName = 'EmptyStateIcon';
