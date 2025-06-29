import { useNavigate } from "react-router";
import { Header } from "../../components/Header/Header";
import { MainContent } from "../../components/Layout/MainContent";
import { MovieDetailsWrapper } from "./DetailPage.internal";
import { SpinnerCircle } from "../../components/Spinner/Spinner.internal";
import Button from "../../components/Button/Button";
import { MoveLeft } from "lucide-react";

interface DetailPageErrorProps {
  type: "invalid" | "loading" | "error";
  errorMessage?: string;
}

export const DetailPageError = (props: DetailPageErrorProps) => {
  const navigate = useNavigate();

  const handleBackClick = () => {
    navigate(-1);
  };

  if (props.type === "invalid") {
    return (
      <>
        <Header>
          <Button title="Go back" $isCircle onClick={handleBackClick}>
            <MoveLeft size={12} color="#333" />
          </Button>
        </Header>
        <MainContent>
          <p>Invalid movie ID.</p>
        </MainContent>
      </>
    );
  }

  if (props.type === "loading") {
    return (
      <>
        <Header>
          <Button title="Go back" $isCircle onClick={handleBackClick}>
            <MoveLeft size={12} color="#333" />
          </Button>
        </Header>
        <MainContent>
          <MovieDetailsWrapper>
            <SpinnerCircle />
            <p>Loading movie details…</p>
          </MovieDetailsWrapper>
        </MainContent>
      </>
    );
  }

  if (props.type === "error") {
    return (
      <>
        <Header>
          <Button title="Go back" $isCircle onClick={handleBackClick}>
            <MoveLeft size={12} color="#333" />
          </Button>
        </Header>
        <MainContent>
          <MovieDetailsWrapper>
            <p>
              Error loading movie details
              {props.errorMessage ? `: ${props.errorMessage}` : "."}
            </p>
          </MovieDetailsWrapper>
        </MainContent>
      </>
    );
  }

  return null;
};
