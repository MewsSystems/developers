import { NavigateFunction } from 'react-router-dom';

export const handleBackNavigation = (navigate: NavigateFunction) => {
  navigate(-1);
};
