// @flow strict

import * as React from "react";
import styled from "styled-components";
import { connect } from "react-redux";
import isEqual from "lodash.isequal";
import Spiner from "./components/Spiner";
import config from "./config";
import Select from "./components/Select";
import { fetchConfigAction, fetchRatesAction } from "./actions/configActions";
import List from "./components/List";
import { fetchRates } from "./services/requests";
import type { ThunkAction } from "./actions/configActions";
import ErrorBox from "./components/ErrorBox";

const Header = styled.header`
  display: flex;
  justify-content: center;
  font-size: 2em;
`;

const Container = styled.div`
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  background-color: ${({ theme }) => theme.colors.color1};
  background: ${({ theme }) =>
    `linear-gradient(to top, ${theme.colors.color5}, ${theme.colors.color1})`};
`;

const Content = styled.div`
  padding: 0 30px;
`;

const Title = styled.h1`
  color: ${({ theme }) => theme.colors.color3};
`;

type Props = {|
  +fetchConfigAction: () => ThunkAction,
  +fetchRatesAction: (ids: string[]) => ThunkAction,
  +selectedRates: string[],
  +isFetchingConfig: boolean,
  +APIerror: ?Error,
  +APIerror: ?Error,
|};

class App extends React.Component<Props> {
  intervalId = null;

  async componentDidUpdate(prevProps, prevState, snapshot) {
    if (this.props.APIerror) {
      this.intervalId && clearInterval(this.intervalId);
    }

    if (!isEqual(prevProps.selectedRates.sort(), this.props.selectedRates.sort())) {
      this.props.fetchRatesAction(this.props.selectedRates);
      this.intervalId && clearInterval(this.intervalId);
      this.intervalId = setInterval(
        () => this.props.fetchRatesAction(this.props.selectedRates),
        config.interval,
      );
    }
  }

  componentDidMount() {
    this.props.fetchConfigAction();
  }

  renderData = () => {
    const { isFetchingConfig } = this.props;
    return isFetchingConfig ? (
      <Spiner />
    ) : (
      <Content>
        <Select />
        <List />
      </Content>
    );
  };

  render() {
    const { APIerror } = this.props;
    return (
      <Container className="App">
        <Header>
          <Title>Exchange Rate App</Title>
        </Header>
        {APIerror ? <ErrorBox error={APIerror} /> : this.renderData()}
      </Container>
    );
  }
}

const mapStateToProps = ({ isFetchingConfig, selectedRates, APIerror }) => ({
  isFetchingConfig,
  selectedRates,
  APIerror,
  APIerror,
});

export default connect(
  mapStateToProps,
  { fetchConfigAction, fetchRatesAction },
)(App);
