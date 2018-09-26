// @flow strict

import * as React from "react";
import styled from "styled-components";
import { connect } from "react-redux";
import isEqual from "lodash.isequal";
import isEmpty from "lodash.isempty";
import Spiner from "./components/Spiner";
import config from "./config";
import Select from "./components/Select";
import List from "./components/List";
import type { ThunkAction } from "./actions/index";
import ErrorBox from "./components/ErrorBox";
import { loadState } from "./localStorage";
import { fetchRatesAction, fetchConfigAction } from "./actions/index";

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
  +fetchConfigConnect: () => ThunkAction,
  +fetchRatesConnect: (ids: string[]) => ThunkAction,
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

  componentDidMount() {
    if (isEmpty(loadState())) {
      this.props.fetchConfigConnect();
    } else {
      this.fetchRates();
    }
  }

  async componentDidUpdate(prevProps) {
    const { APIerror, selectedRates } = this.props;
    const { intervalId } = this.state;
    if (APIerror) {
      // eslint-disable-next-line  no-unused-expressions
      intervalId && clearInterval(intervalId);
    }

    if (!isEqual(prevProps.selectedRates.sort(), selectedRates.sort())) {
      this.fetchRates();
    }
    return null;
  }

  fetchRates = () => {
    const { fetchRatesConnect, selectedRates } = this.props;
    const { intervalId } = this.state;
    fetchRatesConnect(selectedRates);
    // eslint-disable-next-line  no-unused-expressions
    intervalId && clearInterval(intervalId);
    const newIntervalId = setInterval(() => fetchRatesConnect(selectedRates), config.interval);
    this.setState({ intervalId: newIntervalId });
  };

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
});

export default connect(
  mapStateToProps,
  {
    fetchRatesConnect: fetchRatesAction,
    fetchConfigConnect: fetchConfigAction,
  },
)(App);
