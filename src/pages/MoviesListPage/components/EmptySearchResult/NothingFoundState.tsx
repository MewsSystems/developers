import NothingFoundIcon from './icons/NothingFoundIcon';
import {Container, Content, Message, Title} from './styled';

export default function NothingFoundState() {
  return (
    <Container>
      <NothingFoundIcon />
      <Content>
        <Title>Oops...</Title>
        <Message>We could not find any movies matching your search criteria.</Message>
      </Content>
    </Container>
  );
}

NothingFoundState.displayName = 'NothingFoundState';
