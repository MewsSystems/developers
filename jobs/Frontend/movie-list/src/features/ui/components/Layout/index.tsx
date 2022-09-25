import { FC, ReactNode } from "react";
import { VerticalCenter } from "../VerticalCenter";
import { Container } from "./styled";

type Props = {
  children: NonNullable<ReactNode>;
};

export const Layout: FC<Props> = ({ children }) => {
  return (
    <Container>
      <VerticalCenter>{children}</VerticalCenter>
    </Container>
  );
};
