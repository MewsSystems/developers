import { useSimilarMovie } from "../hooks/movies/useSimilarMovie.ts";

const HomePage = () => {
  const { data } = useSimilarMovie(12);

  console.log("a", data);

  return <div>Homepage Homepage</div>;
};

export default HomePage;
