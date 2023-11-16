import styled from "styled-components";
import { useQuery } from "@tanstack/react-query";
import { ShadowBox } from "../../shared/Boxes";
import { ErrorNotification } from "../../shared/ErrorNotification";
import { getMovieDetails } from "../../../queries";
import { HStack, VStack } from "../../Stacks";
import { H1, H3, H4, Section } from "../../shared/Headings";
import { Img } from "../../shared/Image";

interface IMovieDetail {
  selectedMovieId: number;
}

const Info = styled.h3`
  font-family: Exo;
  font-size: 24px;
`;

export function MovieDetail(props: IMovieDetail) {
  const { data, isLoading, isError } = useQuery({
    queryKey: ["movie-detail", props.selectedMovieId],
    queryFn: () => getMovieDetails(props.selectedMovieId),
    staleTime: Infinity,
    refetchOnWindowFocus: false,
    enabled: props.selectedMovieId > 0,
  });
  return (
    <ShadowBox $width="50%">
      {isLoading && <Info>Loading...</Info>}
      {isError && <ErrorNotification />}
      {data != undefined ? (
        <VStack $width="100%">
          <HStack $justifyContent="space-between">
            <VStack $textAlign="left">
              <H1 $fontSize="24px">{data.title}</H1>
              <VStack>
                <H4 $fontSize="16px">Original Title:&nbsp;</H4>
                <H4 $fontSize="16px" $fontWeight="normal">
                  {data.original_title}
                </H4>
              </VStack>
              <VStack>
                <H4 $fontSize="16px">Release Date:&nbsp;</H4>
                <H4 $fontSize="16px" $fontWeight="normal">
                  {data.release_date}
                </H4>
              </VStack>
              <VStack>
                <H4 $fontSize="16px">Genre:&nbsp;</H4>
                <H4 $fontSize="16px" $fontWeight="normal">
                  {data.genres.map((g) => g.name).join(", ")}
                </H4>
              </VStack>
              <VStack>
                <H4 $fontSize="16px">Countries:&nbsp;</H4>
                <H4 $fontSize="16px" $fontWeight="normal">
                  {data.production_countries.map((c) => c.name).join(", ")}
                </H4>
              </VStack>
              <VStack>
                <H4 $fontSize="16px">Languages:&nbsp;</H4>
                <H4 $fontSize="16px" $fontWeight="normal">
                  {data.spoken_languages.map((l) => l.name).join(", ")}
                </H4>
              </VStack>
              <VStack>
                <H4 $fontSize="16px">Collection:&nbsp;</H4>
                <H4 $fontSize="16px" $fontWeight="normal">
                  {data.belongs_to_collection != undefined
                    ? data.belongs_to_collection.name
                    : ""}
                </H4>
              </VStack>
              <VStack>
                <H4 $fontSize="16px">Status:&nbsp;</H4>
                <H4
                  $fontSize="16px"
                  $fontWeight="normal"
                >{`${data.status}`}</H4>
              </VStack>
            </VStack>
            <Img
              $width="300px"
              src={`https://image.tmdb.org/t/p/w300/${data.poster_path}`}
            />
          </HStack>
          <Section>
            <H4 $fontSize="16px">Overview</H4>
            <p>{data.overview}</p>
          </Section>
          <Img
            $width="100%"
            $alignSelf="center"
            src={`https://image.tmdb.org/t/p/w500/${data.backdrop_path}`}
          />
        </VStack>
      ) : (
        <VStack $width="100%">
          <H3 $fontSize="24px">
            Details of the selected movie will appear here...
          </H3>
        </VStack>
      )}
    </ShadowBox>
  );
}
