import {Container, Content, Message, Title} from './styled';
import NothingFoundIcon from './icons/NothingFoundIcon';

export default function NothingFoundState() {
  return (
    <Container>
      <NothingFoundIcon />
      <Content>
        <Title>Oops...</Title>
        <Message>We couldn't find any movies matching your search criteria.</Message>
      </Content>
    </Container>
  );
}

NothingFoundState.displayName = 'NothingFoundState';
