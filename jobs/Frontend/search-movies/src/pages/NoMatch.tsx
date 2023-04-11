import { Link } from "react-router-dom";
import styled from "styled-components";
import { colors } from "../utils/theme";

const NoMatchContainer = styled.div`
  color: ${colors.primaryText};
`;

/**
 * Fallback handler for the route
 * @returns renders if the route is not available
 */
const NoMatch = () => {
  return (
    <NoMatchContainer>
      <h2>Nothing to see here!</h2>
      <p>
        <Link to="/">Go to the home page</Link>
      </p>
    </NoMatchContainer>
  );
};

export default NoMatch;
