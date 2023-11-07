import styled from "styled-components";
import { Button, Rating, Typography } from "../components";

const StyledWrapper = styled.div`
  width: 100vw;
  height: 100vh;
`;

export function Search() {
  return (
    <StyledWrapper>
      <Button>I'm a button</Button>
      <Rating value={4.5} />
      <Typography variant="bodyLarge">Hello</Typography>
      <Typography variant="headlineLarge">Hey yo</Typography>
    </StyledWrapper>
  );
}
