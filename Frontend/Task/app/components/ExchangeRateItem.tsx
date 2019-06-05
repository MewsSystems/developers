import React from "react";
import { CurrencyPair } from "../store/types";
import { TrendEnum } from "./Exchange";
import { CellRow } from "./styled";
import { Cell } from "./Cell";

export interface ExchangeRateItemDTO {
  id: string;
  trend: TrendEnum;
  value: number;
  currencyPair: CurrencyPair[];
}

export interface ExchangeRateItemProps {
  rate: ExchangeRateItemDTO;
}

export const ExchangeRateItem = ({ rate }: ExchangeRateItemProps) => (
  <CellRow>
    <Cell>{`${rate.currencyPair[0].code}/${rate.currencyPair[1].code}`}</Cell>
    <Cell textAlign={"center"}>{rate.value}</Cell>
    <Cell textAlign={"right"}>{rate.trend}</Cell>
  </CellRow>
);
