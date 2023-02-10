import { useNavigate } from "react-router-dom";
import { MovieDetailResult } from "../../app/types";
import { Back, Header, Title } from "./DetailHeader.styled";

type Props = {
  movieDetail: MovieDetailResult;
};

export const DetailHeader = ({ movieDetail }: Props) => {
  const navigate = useNavigate();
  return (
    <Header>
      <Title>
        <h1>{movieDetail.title}</h1>
        {movieDetail.release_date}
      </Title>
      <Back
        className="fa-regular fa-circle-xmark"
        onClick={() => navigate(-1)}
      />
    </Header>
  );
};
