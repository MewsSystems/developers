import { useLocation } from 'react-router-dom';

const usePreviousLocation = () => {
  const location = useLocation();

  return location.state
    ? `${location.state.prevUrl}${location.state.prevSearch}`
    : '/';
};

export default usePreviousLocation;
