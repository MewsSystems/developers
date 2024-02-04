import { Link } from "react-router-dom";
export default function BackButton() {
  // TODO does not keep the search query
  // INFO useNavigate hook like => navigate(-1) can go back outside of the application
  return (
    <Link to=".." className="text-lg font-normal text-gray-300 lg:text-xl">
      <span>&#60;</span> Back to movies list
    </Link>
  );
}
