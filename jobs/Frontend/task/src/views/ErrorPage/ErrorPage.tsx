import { Link, useRouteError } from "react-router-dom";
import { HorizontalCentered } from "src/components/HorizontalCentered/HorizontalCentered";

export const ErrorPage = () => {
  const error = useRouteError();
  console.error(error);

  return (
    <HorizontalCentered id="error-page">
      <h1>Oops!</h1>
      <p>Sorry, page you are looking for does not exist</p>
      <p>
        <i>{error.statusText || error.message}</i>
      </p>
      <Link to="/">Home</Link>
    </HorizontalCentered>
  );
};
