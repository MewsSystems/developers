import {Container, Content, Message, Title} from './EmptyInitialState.styled.tsx';
import EmptyStateIcon from './icons/EmtpyStateIcon.tsx';

export default function EmptyInitialState() {
  return (
    <Container>
      <EmptyStateIcon />
      <Content>
        <Title>Ready to explore?</Title>
        <Message>Your next favorite movie is just a search away!</Message>
      </Content>
    </Container>
  );
}

EmptyInitialState.displayName = 'EmptyInitialState';
