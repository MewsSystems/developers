import {useLocation} from 'react-router-dom';
import {BackLink} from './styled';

type LocationState = {
  previousPath: string;
};

export default function GoBackLink() {
  const location = useLocation();
  const state = location.state as LocationState;

  return (
    <BackLink to={state?.previousPath || '/'}>
      <svg fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path d="M15 6l-6 6 6 6" />
      </svg>
      Back to search
    </BackLink>
  );
}

GoBackLink.displayName = 'GoBackLink';
