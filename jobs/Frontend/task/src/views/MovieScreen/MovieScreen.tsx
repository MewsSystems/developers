import { FC } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { HorizontalCentered } from "src/components/HorizontalCentered/HorizontalCentered";
import { IconButton } from "src/components/IconButton/IconButton";
import { ArrowLeft } from "src/components/icons/ArrowLeft";
import Loader from "src/components/Loader/Loader";
import { Route } from "src/router/Route";
import { useGetMovieByIdQuery } from "src/store/slices/moviesSlice";
import { MovieDetailsType } from "src/store/types/MovieDetailsType";
import { ReduxHookReturn } from "src/store/types/ReduxHookReturn";
import { laptop, mobile, tablet } from "src/styles/appSize";
import { Movie } from "src/views/MovieSearch/components/MovieList/components/Movie";
import styled, { css } from "styled-components";

export const MovieScreen: FC = () => {
  let { movieId } = useParams();
  const navigate = useNavigate();
  const { data, isFetching }: ReduxHookReturn<MovieDetailsType> =
    useGetMovieByIdQuery(movieId);

  if (!data && isFetching) return <Loader loading={isFetching} />;

  const renderGenres = () => {
    return data?.genres.map((genre) => (
      <Genre key={genre.id}>{genre.name}</Genre>
    ));
  };

  return (
    <HorizontalCentered>
      <Header>
        <IconButton name={<ArrowLeft />} onClick={() => navigate(Route.Home)} />
      </Header>
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
          <Overview>{data?.overview}</Overview>
        </InfoWrap>
      </Wrap>
    </HorizontalCentered>
  );
};

const Header = styled.div`
  width: 84%;
  margin-top: 12px;

  ${laptop(css`
    width: 100%;
  `)};

  ${tablet(css`
    width: 80%;
  `)};

  ${mobile(css`
    width: 100%;
  `)};
`;

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

  ${laptop(css`
    width: 100%;
  `)};

  ${tablet(css`
    width: 80%;
    flex-direction: column;
    align-items: center;
    margin-top: 0;
  `)};

  ${mobile(css`
    width: 100%;
    flex-direction: column;
    align-items: center;
    padding-top: 0;
  `)};
`;

const InfoWrap = styled.div`
  width: calc(100% - 300px);

  ${laptop(css`
    width: 100%;
  `)};
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

const Overview = styled.p`
  ${laptop(css`
    font-size: 14px;
  `)};
`;
