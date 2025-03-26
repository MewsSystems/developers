import { NavLink } from "react-router";

const NotFoundPage = () => {
  return (
    <>
      <div>Ooops, you have stumbled into the void. The page you are looking for does not exist.</div>
      <NavLink to="/">Return home</NavLink>
    </>
  );
};

export default NotFoundPage;

