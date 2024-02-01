import { useParams } from "react-router-dom";
export default function MovieDetail() {
  const params = useParams();
  console.log(params.movieId);
  return (
    <h2 className="text-3xl font-bold underline">
      Hello detail - {params.movieId} !
    </h2>
  );
}
