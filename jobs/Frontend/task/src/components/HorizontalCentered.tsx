import { FC, ReactNode } from "react";
import styled from "styled-components";

export const HorizontalCentered: FC<{ children: ReactNode }> = ({
  children,
}) => {
  return <SearchWrap>{children}</SearchWrap>;
};

const SearchWrap = styled.div`
  display: flex;
  justify-content: center;
`;
