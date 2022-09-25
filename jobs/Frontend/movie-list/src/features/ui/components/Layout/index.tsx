import { FC, ReactNode } from "react";

type Props = {
  children: NonNullable<ReactNode>;
};

export const Layout: FC<Props> = ({ children }) => {
  return <div>{children}</div>;
};
