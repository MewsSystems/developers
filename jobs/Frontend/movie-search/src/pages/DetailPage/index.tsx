import { useNavigate, useParams } from "react-router";
import { Header } from "../../components/Header/Header";
import { MainContent } from "../../components/Layout/MainContent";
import { Heading } from "../../components/Typography/Heading";

export const DetailPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  return (
    <>
      <Header>
        <button onClick={() => navigate(-1)}>back</button>
      </Header>
      <MainContent>
        <Heading>{id}</Heading>
      </MainContent>
    </>
  );
};
