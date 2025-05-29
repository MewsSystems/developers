import {Container, Content, Message, Title} from './NothingFoundState.styled.tsx';
import {NothingFoundIcon} from './icons/NothingFoundIcon.tsx';

export const NothingFoundState = () => {
  return (
    <Container>
      <NothingFoundIcon />
      <Content>
        <Title>Oops...</Title>
        <Message>We couldn't find any movies matching your search criteria.</Message>
      </Content>
    </Container>
  );
};

NothingFoundState.displayName = 'NothingFoundState';
