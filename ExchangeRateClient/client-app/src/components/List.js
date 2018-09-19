// @flow strict

import * as React from "react";
import styled from "styled-components";
import { connect } from "react-redux";
import { FaAngleDoubleUp } from "react-icons/fa";
import Rate from "./Rate";
import createRateLabel from "../utils/createRateLabel";
import theme from "../theme";
import type { Theme } from "../theme/types";
import type { Rates, Data, SelectedRates } from "../store/types";

const Item = styled.li`
  display: flex;
  align-items: center;
  justify-content: flex-start;
  font-size: 2rem;
  padding: 0.5rem 1rem;
  opacity: 0.7;
  background-color: ${({ isOdd, theme }: { isOdd: boolean, theme: Theme }) =>
    isOdd ? theme.colors.color6 : "grey"};
`;

Item.defaultProps = {
  theme,
};

const ListContainer = styled.ul`
  margin: 2em;
  list-style-type: none;
`;

const Sublabel = styled.span`
  font-size: 15px;
  color: ${({ theme }: { theme: Theme }) => theme.colors.white};
`;

Sublabel.defaultProps = {
  theme,
};

const Label = styled.span`
  flex: 1;
  display: flex;
  flex-direction: column;
`;

type Props = {
  selectedRates: SelectedRates,
  data: Data,
  rates: Rates,
};

const List = ({ data, selectedRates, rates }: Props) => (
  <ListContainer>
    {Object.keys(rates).map((id, index) => (
      <Item key={id} isOdd={index % 2 == 0}>
        <Label>
          {`${createRateLabel(data[id].currencies)}:`}
          <Sublabel>
            {data[id].currencies[0].name} - {data[id].currencies[1].name}
          </Sublabel>
        </Label>
        <Rate current={rates[id].current} before={rates[id].before} />
      </Item>
    ))}
  </ListContainer>
);

const mapStateToProps = ({ data, selectedRates, rates }) => ({
  rates,
  data,
  selectedRates,
});

export default connect(mapStateToProps)(List);
