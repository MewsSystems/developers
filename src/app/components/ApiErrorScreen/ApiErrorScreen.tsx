import {useNavigate} from 'react-router-dom';
import {PageType, PathByPageType} from '../../../routes/constants';
import {Button, Container, ErrorIcon, Message, Title} from './styled';

type ErrorScreenProps = {
  errorMessage: string;
  onReset?: () => void;
};

export default function ApiErrorScreen({errorMessage, onReset}: ErrorScreenProps) {
  const navigate = useNavigate();

  const goBackHome = () => {
    onReset?.();
    navigate(PathByPageType[PageType.MoviesListPage]);
  };

  return (
    <Container>
      <ErrorIcon>⚠️</ErrorIcon>
      <Title>Oops! Something went wrong</Title>
      <Message>{errorMessage}</Message>
      <Button onClick={goBackHome}>Go to Homepage</Button>
    </Container>
  );
}
