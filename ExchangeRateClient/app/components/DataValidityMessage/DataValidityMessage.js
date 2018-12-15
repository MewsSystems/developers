import React from "react";
import styled from "styled-components";

const Container = styled.span`
  padding-bottom: 15px;
`;

const Label = styled.span``;

const Time = styled.span`
  margin-left: 8px;
  font-weight: 500;
`;

const DataValidityMessage = ({ time }) => (
  <Container>
    <Label>Last updated at</Label>
    <Time>{new Date(time).toLocaleTimeString()}</Time>
  </Container>
);

export default DataValidityMessage;
