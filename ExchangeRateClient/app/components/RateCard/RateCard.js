import React, { Component } from "react";
import styled from "styled-components";
import TrendGrowingSvg from "./trend_growing.svg";
import TrendDecliningSvg from "./trend_declining.svg";

const Container = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: space-around;
  background: #ffffff;
  width: 170px;
  height: 170px;
  border-radius: 5px;
  box-shadow: 2px 4px 25px rgba(138, 138, 138, 0.1);
`;

const CurrencyPairLabel = styled.span`
  font-weight: 800;
  font-size: 24px;
  letter-spacing: 0.02em;
  color: #2e2e2e;
`;

const ExchangeRateContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
`;

const ExchangeRateLabel = styled.span`
  font-size: 11px;
  color: #8a8a8a;
`;

const ExchangeRateValueContainer = styled.div`
  display: flex;
  align-items: center;
`;

const ExchangeRateIconContainer = styled.div`
  margin-left: 5px;
  color: #8a8a8a;
`;

const ExchangeRateValue = styled.span`
  font-weight: bold;
  line-height: normal;
  font-size: 22px;
  color: #272727;
`;

const getTrendIcon = (previousRate, currentRate) => {
  if (previousRate && previousRate < currentRate) {
    return <TrendGrowingSvg />;
  } else if (previousRate && previousRate > currentRate) {
    return <TrendDecliningSvg />;
  } else {
    return "-";
  }
};

const RateCard = ({ currencyPair }) => (
  <Container>
    <CurrencyPairLabel>
      {currencyPair.currency1.code} / {currencyPair.currency2.code}
    </CurrencyPairLabel>
    <ExchangeRateContainer>
      <ExchangeRateLabel>Exchange rate</ExchangeRateLabel>
      <ExchangeRateValueContainer>
        <ExchangeRateValue>
          {currencyPair.currentRate
            ? currencyPair.currentRate.toFixed(4)
            : null}
        </ExchangeRateValue>
        <ExchangeRateIconContainer>
          {getTrendIcon(currencyPair.previousRate, currencyPair.currentRate)}
        </ExchangeRateIconContainer>
      </ExchangeRateValueContainer>
    </ExchangeRateContainer>
  </Container>
);

export default RateCard;
