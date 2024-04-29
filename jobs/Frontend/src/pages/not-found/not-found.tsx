import * as React from 'react';
import Feedback from '../../shared/components/feedback/feedback';

const NotFound = () => {
  return (
    <Feedback title="404 - Not found" subtitle={'The page does not exists'} />
  );
};

export default NotFound;
