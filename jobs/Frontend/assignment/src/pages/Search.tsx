import styled from "styled-components";
import { Button, Chip, Rating, Typography, Input } from "../components";
import NetflixLogo from "@/assets/logos/netflix.png";
import { useState } from "react";

const StyledWrapper = styled.div`
  width: 100vw;
  height: 100vh;

  padding: 16px;

  display: flex;
  flex-direction: column;
  gap: 16px;

  // background-color: lightgrey;
`;

export function Search() {
  const [value, setValue] = useState("");

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
      <Input label="Placeholder" value={value} onChange={setValue} />
    </StyledWrapper>
  );
}
