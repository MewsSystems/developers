import { AutocompleteSearcher } from "./autocomplete-searcher";
import styled from "styled-components";

export const DiscoverBox: React.FC<{ width: string }> = ({ width }) => {
  return (
    <Container width={width}>
      <Title>Discover Your Next Favorite Movie</Title>
      <Description>
        Search through thousands of films to find the perfect match for your
        next movie night.
      </Description>
      <SearchContainer>
        <AutocompleteSearcher
          color="#4B5563"
          size="large"
          placeholder="Search your movie here..."
        />
      </SearchContainer>
    </Container>
  );
};

const Container = styled.div<{ width: string }>`
  display: flex;
  flex-direction: column;
  background-color: #c4ab9c;
  width: ${(props) => props.width};
  border-radius: 16px;
  align-items: center;
  justify-content: center;
  padding: 20px;
  margin-top: 150px;
`;

const Title = styled.h1`
  font-size: 1.5rem;
  font-weight: bold;
  text-align: center;
  color: #1f2937;
`;

const Description = styled.h6`
  font-size: 1rem;
  text-align: center;
  color: #4b5563;
  margin-top: 8px;
`;

const SearchContainer = styled.div`
  width: 100%;
  height: 40px;
  margin: 20px 0;
`;
