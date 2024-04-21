/* Global imports */
import styled from "styled-components";
import { Link, useParams } from "wouter";
import { useMovieDetail } from "../hooks/useMovieDetail";
import { imageDomainURL } from "../utils/constant";
import { LoadingMessage } from "../components/ui/LoadingMessage";
import {
  Text,
  Column,
  Row,
  Separator,
  Wrapper,
  Gap,
  Title,
} from "../components/ui/Layout";
import { motion } from "framer-motion";

/* Local imports */

/* Types  */
/* Local utility functions */
const separateIntoCommas = (strings: string[]) => {
  const strWithCommas = strings.map((str) => str.concat(",")).join(" ");
  return strWithCommas.trim().lastIndexOf(",") === strWithCommas.length - 1
    ? strWithCommas.slice(0, -1)
    : strWithCommas;
};
/* Component definition */
export const MoviesDetailView = () => {
  const params = useParams<{ id: string }>();
  const { movieDetail, isLoading } = useMovieDetail(Number(params.id));

  return (
    <MovieDetailLayout>
      <Column>
        <Link to={".."}>
          <Text color="white">Back</Text>
        </Link>
        <Gap />
        {isLoading ? (
          <LoadingMessage text="Loading movie detail..." />
        ) : movieDetail ? (
          <Row>
            <Poster
              key="detail"
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              exit={{ opacity: 0 }}
            >
              <motion.img
                style={{ borderRadius: "10px" }}
                src={`${imageDomainURL}w400/${movieDetail?.poster_path}`}
                alt={movieDetail?.title}
                animate={{ scale: [0, 1] }}
              />
            </Poster>
            <Content animate={{ opacity: [0, 1] }}>
              <Wrapper>
                <Title>{movieDetail?.title}</Title>
                <Text color="white" size="lg">
                  🎖️{movieDetail?.popularity}
                </Text>
              </Wrapper>
              <Wrapper>
                <Text color="white">
                  🗓️{" "}
                  {new Date(
                    movieDetail?.release_date ?? ""
                  ).toLocaleDateString()}
                  -💰{" "}
                  {new Intl.NumberFormat("en", {
                    style: "currency",
                    currency: "USD",
                  }).format(movieDetail?.budget)}
                </Text>
              </Wrapper>
              <Wrapper>
                <Text color="white">{movieDetail.overview}</Text>
              </Wrapper>
              <Separator />
              <Wrapper>
                <Text size="lg">
                  Genre:{" "}
                  <Text color="white">
                    {separateIntoCommas(
                      movieDetail.genres.map((gen) => gen.name)
                    )}
                  </Text>
                </Text>
              </Wrapper>
            </Content>
          </Row>
        ) : (
          <Text size="xl">No data found</Text>
        )}
      </Column>
    </MovieDetailLayout>
  );
};
const MovieDetailLayout = styled(motion.div)`
  display: flex;
  padding: 1rem;
  height: 100vh;
  width: 100%;
  flex-direction: column;
`;

const Poster = styled(motion.section)`
  display: flex;
  justify-content: center;
`;
const Content = styled(motion.section)`
  padding: 1rem;
  width: 100%;
  display: flex;
  flex: 1;
  flex-direction: column;
`;
