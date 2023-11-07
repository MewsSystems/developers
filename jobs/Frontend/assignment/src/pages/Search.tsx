import styled from "styled-components";
import { Button, Chip, Rating, Typography } from "../components";
import NetflixLogo from "@/assets/logos/netflix.png";

const StyledWrapper = styled.div`
  width: 100vw;
  height: 100vh;

  background-color: lightgrey;
`;

export function Search() {
  return (
    <StyledWrapper>
      <Button>I'm a button</Button>
      <Rating value={4.5} />
      <Typography variant="bodyLarge">Hello</Typography>
      <Typography variant="headlineLarge">Hey yo</Typography>
      <div style={{ display: "flex", gap: 8 }}>
        <Chip label="Thriller chip" />
        <Chip label="Thriller chip" />
        <Chip label="Thriller chip" imagePath={NetflixLogo} />
      </div>
    </StyledWrapper>
  );
}
