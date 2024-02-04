import MovieDetail from "../components/MovieDetail/MovieDetail";
import Constants from "../config/constants";

export default function MovieDetailPage() {
  return <MovieDetail />;
}

export function movieDetailLoader({ request, params }) {
  console.log(request, params);

  var requestOptions = {
    method: "GET",
    redirect: "follow",
  };

  return fetch(
    `${Constants.API_URL}/${Constants.API_VERSION}/movie/${params.movieID}?api_key=${Constants.API_KEY}`,
    requestOptions
  );
}
