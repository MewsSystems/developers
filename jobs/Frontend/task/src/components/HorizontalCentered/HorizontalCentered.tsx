import { FC, ReactNode } from "react";
import { tablet } from "src/styles/appSize";
import styled, { css } from "styled-components";

export const HorizontalCentered: FC<{ children: ReactNode }> = ({
  children,
}) => {
  return <DivWrap>{children}</DivWrap>;
};

const DivWrap = styled.div`
  max-width: 1200px;
  margin: 0 auto;
  display: flex;
  align-items: center;
  flex-direction: column;
  padding: 0 10%;

  ${tablet(css`
    padding: 0 36px;
  `)};
`;
