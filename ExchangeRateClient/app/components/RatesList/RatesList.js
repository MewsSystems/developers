import React from "react";
import styled from "styled-components";

import RateCard from "../RateCard";

const Container = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, 170px);
  grid-gap: 1rem;
  margin: 30px 0;
`;

const RatesList = ({ currencyPairs }) => (
  <Container>
    {currencyPairs.map(currencyPair => (
      <RateCard currencyPair={currencyPair} key={currencyPair.id} />
    ))}
  </Container>
);

export default RatesList;
