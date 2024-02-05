import MovieDetail from "../components/MovieDetail/MovieDetail";
import Constants from "../config/constants";
import BackButton from "../components/BackButton";

export default function MovieDetailPage() {
  return (
    <>
      <BackButton className="mb-8" />
      <MovieDetail />
    </>
  );
}

export function movieDetailLoader({ params }) {
  const requestOptions: RequestInit = {
    method: "GET",
    redirect: "follow",
  };

  // INFO: Error handling will be done in errorElement of the router
  return fetch(
    `${Constants.API_URL}/${Constants.API_VERSION}/movie/${params.movieID}?api_key=${Constants.API_KEY}`,
    requestOptions
  );
}
