import { useParams } from "react-router-dom";
import {
  BottomBar,
  Credits,
  MaxWidthWrapper,
  MovieImages,
  MovieOverview,
  SectionWrapper,
  SimilarMovies,
  Typography,
} from "@/components";
import styled from "styled-components";
import { useScrollToTop } from "@/hooks";

const DetailsWrapper = styled.div`
  padding: 40px 23px 100px 23px;
  background-color: ${({ theme }) => theme.colors.surface.main};
`;

export function Details() {
  const { id } = useParams();
  const parsedId = Number(id);

  useScrollToTop(id);

  return (
    <div>
      <MovieOverview movieId={parsedId} />
      <DetailsWrapper>
        <MaxWidthWrapper>
          <SectionWrapper>
            <Typography variant="headlineLarge" bold>
              Media:
            </Typography>
            <MovieImages movieId={parsedId} />
          </SectionWrapper>
          <SectionWrapper>
            <Typography variant="headlineLarge" bold>
              Similar movies:
            </Typography>
            <SimilarMovies movieId={parsedId} />
          </SectionWrapper>
          <SectionWrapper>
            <Typography variant="headlineLarge" bold>
              Cast & Crew:
            </Typography>
            <Credits movieId={parsedId} />
          </SectionWrapper>
        </MaxWidthWrapper>
      </DetailsWrapper>
      <BottomBar />
    </div>
  );
}
