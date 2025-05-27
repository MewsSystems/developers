import { useNavigate } from 'react-router-dom';
import styled from 'styled-components';

const NotFoundContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  padding: 2rem;
  text-align: center;
  background-color: #f8f9fa;
`;

const Title = styled.h1`
  font-size: 6rem;
  font-weight: bold;
  color: #343a40;
  margin: 0;
  line-height: 1;
`;

const Subtitle = styled.h2`
  font-size: 2rem;
  color: #6c757d;
  margin: 1rem 0 2rem;
`;

const Message = styled.p`
  font-size: 1.2rem;
  color: #495057;
  margin-bottom: 2rem;
  max-width: 600px;
`;

const BackButton = styled.button`
  padding: 0.8rem 2rem;
  font-size: 1.1rem;
  color: white;
  background-color: #007bff;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.2s;

  &:hover {
    background-color: #0056b3;
  }
`;

export const NotFoundComponent = () => {
  const navigate = useNavigate();

  return (
    <NotFoundContainer>
      <Title>404</Title>
      <Subtitle>Page Not Found</Subtitle>
      <Message>
        Oops! The page you're looking for seems to have vanished into thin air.
        Don't worry, you can always go back to the home page and continue your movie journey.
      </Message>
      <BackButton onClick={() => navigate('/')}>
        Back to Home
      </BackButton>
    </NotFoundContainer>
  );
}; 