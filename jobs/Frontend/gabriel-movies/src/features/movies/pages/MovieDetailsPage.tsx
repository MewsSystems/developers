import styled from "styled-components";
import { useParams } from "react-router-dom";
import { Page } from "@/shared/ui/atoms/Layout/Page";
import { Title } from "@/shared/ui/atoms/Typography/Title";
import { Text } from "@/shared/ui/atoms/Typography/Text";
import { Row } from "@/shared/ui/atoms/Layout/Row";
import { useFetchMovie } from "../hooks/useFetchMovie";
import { UserScore } from "../components/UserScore";

const Wrap = styled.section`
  display: grid;
  grid-template-columns: auto 1fr;
  gap: 24px;

  @media (max-width: 768px) {
    grid-template-columns: 1fr;
    justify-items: center;
    gap: 16px;
  }
`;

const WrapDetails = styled.div`
  text-align: left;
`;

const DateText = styled.p`
  color: ${({ theme }) => theme.colors.textLight};
`;

const Poster = styled.img`
  width: 280px;                          
  max-width: 100%;
  height: auto;
  border-radius: 12px;
  display: block;
`;

export default function MovieDetailPage() {
  const { id } = useParams<{ id: string }>();
  const { data: movie } = useFetchMovie(id);

  return (
    <Page>
      <Wrap>
        <Poster
          src={movie.posterPath}
          alt={`${movie.title} poster`}
          loading="eager"
        />

        <WrapDetails>
          <Title>{movie.title}</Title>
          <Row>
            <DateText>{movie.formattedReleaseDate}</DateText>
            <UserScore voteAverage={movie.voteAverage} />
          </Row>
          <Text>{movie.overview}</Text>
        </WrapDetails>
      </Wrap>
    </Page>
  );
}
