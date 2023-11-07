import styled from "styled-components";
import { Button } from "../components";

const StyledWrapper = styled.div`
  width: 100vw;
  height: 100vh;
`;

export function Search() {
  return (
    <StyledWrapper>
      <Button>I'm a button</Button>
    </StyledWrapper>
  );
}
