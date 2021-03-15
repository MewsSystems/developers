import { ChevronLeft } from '@styled-icons/fa-solid';
import { ReactElement } from 'react';
import { useHistory } from 'react-router-dom';
import { Button } from './styled';

function BackButton(): ReactElement {
  const history = useHistory();
  return (
    <Button type="button" onClick={history.goBack}>
      <ChevronLeft /> Back
    </Button>
  );
}

export default BackButton;
