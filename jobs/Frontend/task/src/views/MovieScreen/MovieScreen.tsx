import { FC } from "react";
import { useParams } from "react-router-dom";
import { HorizontalCentered } from "src/components/HorizontalCentered/HorizontalCentered";
import Loader from "src/components/Loader/Loader";
import { useGetMovieByIdQuery } from "src/store/slices/moviesSlice";
import { MovieDetailsType } from "src/store/types/MovieDetailsType";
import { MovieType } from "src/store/types/MovieType";
import { ReduxHookReturn } from "src/store/types/ReduxHookReturn";
import { Movie } from "src/views/MovieSearch/components/MovieList/components/Movie";
import styled from "styled-components";

export const MovieScreen: FC = () => {
  let { movieId } = useParams();
  const { data, isFetching }: ReduxHookReturn<MovieDetailsType> =
    useGetMovieByIdQuery(movieId);

  console.log(data);

  if (!data && isFetching) return <Loader loading={isFetching} />;

  const renderGenres = () => {
    return data?.genres.map((genre) => (
      <Genre key={genre.id}>{genre.name}</Genre>
    ));
  };

  return (
    <HorizontalCentered>
      <Wrap>
        <Movie movie={data} imgSize={"300"} disableLink={true} />
        <InfoWrap>
          <Title>
            <span>{data?.title}</span>
            <OriginalTitle
              href={`https://www.imdb.com/title/${data?.imdb_id}/`}
              target="_blank"
              rel="noopener noreferrer"
            >{`( ${data?.original_title} )`}</OriginalTitle>
          </Title>
          <GenresWrap>
            <i>{renderGenres()}</i>
          </GenresWrap>
          <p>{data?.overview}</p>
        </InfoWrap>
      </Wrap>
    </HorizontalCentered>
  );
};

const Wrap = styled.div`
  width: 80%;
  margin-top: 24px;
  display: flex;
  gap: 24px;
  background: #333333;
  padding: 24px;
  box-shadow: 8px 8px 10px 0px rgba(0, 0, 0, 0.3);

  h2 {
    margin: 0;
  }
`;

const InfoWrap = styled.div`
  width: calc(100% - 300px);
`;

const OriginalTitle = styled.a`
  font-size: 14px;
  margin-left: 10px;
  margin-top: 6px;
  color: white;
`;

const Title = styled.h2`
  display: flex;
  border-bottom: 1px solid #4c4c4c;
`;

const Genre = styled.span`
  border: 1px solid gray;
  padding: 4px 8px;
  border-radius: 8px;
  margin-right: 8px;

  &:last-child {
    margin-right: 8px;
  }
`;

const GenresWrap = styled.div`
  margin-top: 20px;
`;
