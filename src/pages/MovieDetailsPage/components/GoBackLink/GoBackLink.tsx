import {useSearchParams} from 'react-router-dom';
import {BackLink} from './GoBackLink.styled';

export default function GoBackLink() {
  const [searchParams] = useSearchParams();

  const backToSearchUrl = `/?${searchParams.toString()}`;

  return (
    <BackLink to={backToSearchUrl}>
      <svg fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path d="M15 6l-6 6 6 6" />
      </svg>
      Back to search
    </BackLink>
  );
}

GoBackLink.displayName = 'GoBackLink';
