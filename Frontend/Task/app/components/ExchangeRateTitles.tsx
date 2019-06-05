import React from "react";
import { CellRow } from "./styled";
import { Cell } from "./Cell";

export const ExchangeRateTitles = () => (
  <CellRow>
    <Cell isTitle>Currencies</Cell>
    <Cell isTitle textAlign={"center"}>
      Rate
    </Cell>
    <Cell isTitle textAlign={"right"}>
      Trend
    </Cell>
  </CellRow>
);
