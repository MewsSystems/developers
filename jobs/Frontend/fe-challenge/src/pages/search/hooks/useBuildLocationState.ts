import { useLocation } from 'react-router-dom';

const useBuildLocationState = () => {
  const location = useLocation();

  return { prevUrl: location.pathname, prevSearch: location.search };
};

export default useBuildLocationState;
