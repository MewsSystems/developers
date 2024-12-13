import { Link } from "react-router";
import {
  NoSuchMovieContainer,
  NoSuchMovieIcon,
  NoSuchMovieMessage,
} from "@/components/NoMovieFound/NoMovieFoundStyle";
import { BackButton } from "@/pages/MovieDetails/MovieDetailsStyle";

interface NoMovieFoundProps {
  message?: string;
}
const NoMovieFound: React.FC<NoMovieFoundProps> = ({
  message = "No such movie, sorry",
}) => {
  return (
    <NoSuchMovieContainer>
      <NoSuchMovieIcon>😥</NoSuchMovieIcon>
      <NoSuchMovieMessage>{message}</NoSuchMovieMessage>
      <Link to="/">
        <BackButton>← Back to Search</BackButton>
      </Link>
    </NoSuchMovieContainer>
  );
};

export default NoMovieFound;
