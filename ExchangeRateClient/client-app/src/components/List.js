// @flow strict

import * as React from "react";
import styled from "styled-components";
import { connect } from "react-redux";
import Rate from "./Rate";
import createRateLabel from "../utils/createRateLabel";
import defaultTheme from "../theme";
import type { Theme } from "../theme/types";
import type { Rates, Data } from "../store/types";

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
  theme: defaultTheme,
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
  theme: defaultTheme,
};

const Label = styled.span`
  flex: 1;
  display: flex;
  flex-direction: column;
`;

type Props = {
  data: Data,
  rates: Rates,
};

const List = ({ data, rates }: Props) => (
  <ListContainer>
    {Object.keys(rates).map((id, index) => (
      <Item key={id} isOdd={index % 2 === 0}>
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

const mapStateToProps = ({ data, rates }) => ({
  rates,
  data,
});

export default connect(mapStateToProps)(List);
