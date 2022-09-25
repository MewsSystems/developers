import { FC, ReactNode } from "react";

type Props = {
  children: NonNullable<ReactNode>;
};

export const MovieListItem: FC<Props> = () => {
  return <div>ListItem</div>;
};
