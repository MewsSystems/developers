// @flow strict

import * as React from "react";
import styled from "styled-components";
import { connect } from "react-redux";
import isEqual from "lodash.isequal";
import isEmpty from "lodash.isempty";
import Spiner from "./components/Spiner";
import config from "./config";
import Select from "./components/Select";
import { fetchConfigAction, fetchRatesAction } from "./actions/index";
import List from "./components/List";
import { fetchRates } from "./services/requests";
import type { ThunkAction } from "./actions/index";
import ErrorBox from "./components/ErrorBox";
import { loadState, saveState } from "./localStorage";

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

type State = {|
  intervalId: ?IntervalID,
|};

class App extends React.Component<Props, State> {
  state = { intervalId: null };

  async componentDidUpdate(prevProps, prevState, snapshot) {
    if (this.props.APIerror) {
      this.state.intervalId && clearInterval(this.state.intervalId);
      // this.setState({ intervalId: null });
    }

    if (!isEqual(prevProps.selectedRates.sort(), this.props.selectedRates.sort())) {
      this.fetchRates();
    }
  }

  fetchRates = () => {
    this.props.fetchRatesAction(this.props.selectedRates);
    this.state.intervalId && clearInterval(this.state.intervalId);
    const newIntervalId = setInterval(
      () => this.props.fetchRatesAction(this.props.selectedRates),
      config.interval,
    );
    this.setState({ intervalId: newIntervalId });
  };

  componentDidMount() {
    if (isEmpty(loadState())) {
      this.props.fetchConfigAction();
    } else {
      this.fetchRates();
    }
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
    console.log(this.state);
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
