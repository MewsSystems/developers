import { Link, useLocation } from "react-router-dom";
export default function BackButton({ className: string }) {
  // TODO does not keep the search query
  // INFO useNavigate hook like => navigate(-1) can go back outside of the application
  const location = useLocation();
  const query = location?.state?.query;

  const url = query ? `../${query}` : "..";
  return (
    <Link to={url} className="text-lg font-normal text-gray-300 lg:text-xl">
      <span>&#60;</span> Back to movies list
    </Link>
  );
}
