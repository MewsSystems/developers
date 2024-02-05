import MovieDetail from "../components/MovieDetail/MovieDetail";
import Constants from "../config/constants";
import BackButton from "../components/BackButton";

export default function MovieDetailPage() {
  return (
    <>
      <div className="mb-8">
        <BackButton />
      </div>
      <MovieDetail />;
    </>
  );
}

export function movieDetailLoader({ request, params }) {
  console.log(request, params);

  const requestOptions: RequestInit = {
    method: "GET",
    redirect: "follow",
  };

  return fetch(
    `${Constants.API_URL}/${Constants.API_VERSION}/movie/${params.movieID}?api_key=${Constants.API_KEY}`,
    requestOptions
  );
}
