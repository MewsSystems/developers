import { Link, useLocation } from "react-router-dom";
export default function BackButton({ className: string }) {
  const location = useLocation();
  // INFO won´t be present when accessing directly the movie detail from outside of the application - going back will simple go one level up without keeping the query
  const previousQueryParams = location?.state?.query;

  const url = previousQueryParams ? `../${previousQueryParams}` : "..";
  return (
    <Link to={url} className="text-lg font-normal text-gray-300 lg:text-xl">
      <span>&#60;</span> Back to movies list
    </Link>
  );
}
