import EmptyStateIcon from './icons/EmptyStateIcon';
import {Container, Content, Message, Title} from './styled';

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
