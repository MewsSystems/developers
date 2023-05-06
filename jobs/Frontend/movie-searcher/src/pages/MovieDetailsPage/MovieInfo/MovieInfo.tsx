import { StarOutlined } from "@ant-design/icons";
import { Divider } from "antd";
import {
  Title,
  ColStyled,
  SmallTitle,
  OriginalTitle,
  Overview,
  RatingWrapper,
  Rating,
  HeaderWrapper,
  FlexWrapper,
  VoteCount,
} from "./MovieInfo.styled";

type MovieInfoProps = {
  title: string;
  releaseDate: string;
  overview: string;
  budget: string | null;
  originalTitle: string;
  rating: number;
  voteCount: string | null;
};

const MovieInfo = ({
  title,
  releaseDate,
  overview,
  budget,
  originalTitle,
  rating,
  voteCount,
}: MovieInfoProps) => (
  <ColStyled span={14}>
    <HeaderWrapper>
      <div>
        <Title>{title}</Title>
        <OriginalTitle>{originalTitle}</OriginalTitle>
      </div>
      <div>
        <RatingWrapper>
          <StarOutlined />
          <Rating>
            {rating}
            <span>/10</span>
          </Rating>
        </RatingWrapper>
        <VoteCount>({voteCount} votes)</VoteCount>
      </div>
    </HeaderWrapper>

    <Divider />

    <FlexWrapper>
      <SmallTitle>Release:</SmallTitle>
      <span>{releaseDate}</span>
    </FlexWrapper>
    {budget && (
      <FlexWrapper>
        <SmallTitle>Budget:</SmallTitle>
        <span>{budget}</span>
      </FlexWrapper>
    )}

    <Divider />

    <Overview>{overview}</Overview>
  </ColStyled>
);

export { MovieInfo };
export type { MovieInfoProps };
