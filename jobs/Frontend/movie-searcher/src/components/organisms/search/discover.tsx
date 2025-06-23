import { DiscoverBox } from "../../molecules/search/discover-box";
import hollywoodMovies from "../../../assets/images/hollywood_movies.jpg";
import styled from "styled-components";

export const HomeDiscover: React.FC = () => {
  return (
    <Container>
      <BackgroundImage src={hollywoodMovies} alt="hollywoodMovies" />
      <Overlay />
      <DiscoverBoxContainer>
        <DiscoverBox width="70%" />
      </DiscoverBoxContainer>
    </Container>
  );
};

const Container = styled.div`
  position: relative;
`;

const BackgroundImage = styled.img`
  width: 100%;
  height: 450px;
`;

const Overlay = styled.div`
  position: absolute;
  inset: 0;
  background: linear-gradient(
    to bottom,
    rgba(0, 0, 0, 0.9),
    rgba(0, 0, 0, 0.8),
    rgba(0, 0, 0, 1)
  );
`;

const DiscoverBoxContainer = styled.div`
  position: absolute;
  inset: 0;
  display: flex;
  align-items: center;
  justify-content: center;
`;
