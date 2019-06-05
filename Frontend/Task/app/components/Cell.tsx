import React from "react";
import { CellTitle, Paragraph } from "./styled";
import { CellStyled } from "./styled";
import { TypographyProps } from "./styled";

interface CellProps {
  isTitle?: boolean;
  children: React.ReactNode;
}

type AllProps = TypographyProps & CellProps;

export const Cell = ({ children, isTitle, textAlign }: AllProps) => (
  <CellStyled>
    {isTitle ? (
      <CellTitle textAlign={textAlign}>{children}</CellTitle>
    ) : (
      <Paragraph textAlign={textAlign}>{children}</Paragraph>
    )}
  </CellStyled>
);
