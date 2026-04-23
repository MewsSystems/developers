import { Button } from "@/components/ui/Button";
import { NavLink } from "react-router";

const NotFoundPage = () => {
  return (
    <>
      <h1 className="mb-6">404</h1>
      <p className="text-xl mb-6">
        Oops! Looks like you stumbled into a void. The page you are looking for doesn't exist.
      </p>
      <NavLink
        to="/"
      >
        <Button>
          Return to Home
        </Button>
      </NavLink>
    </>
  );
};

export default NotFoundPage;

