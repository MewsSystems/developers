import styled from "styled-components"

import { mq, ScreenSize } from "~/features/ui/theme/mq"

export const Container = styled.div`
  --horizontal-spacing: 0.8rem;

  margin: 0 auto;
  padding: 0 var(--horizontal-spacing);
  max-width: calc(${ScreenSize.large / 10}rem + 2 * var(--horizontal-spacing));
  width: 100%;

  ${mq.medium} {
    --horizontal-spacing: 2rem;
  }

  ${mq.large} {
    --horizontal-spacing: 4rem;
  }
`
