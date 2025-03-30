import { Link } from 'react-router-dom';

export const BackToHPButton = () => {
  return (
    <Link
      className="gradient-hover f-link-md"
      to="/"
      aria-label="Back to search"
    >
      ← Back to Movie Search
    </Link>
  );
};
