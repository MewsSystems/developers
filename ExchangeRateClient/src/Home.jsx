// @flow
import React, { Component } from 'react';
import { List } from 'immutable';
import { connect } from 'react-redux';
import styled from 'styled-components';

import type { CurrencyPairRate } from './types';
import type { RequestStatus, CurrencyPair } from './reducer';
import { getCurrencyPairList, getCurrencyPairRates } from './selectors';
import { fetchConfig, fetchRates } from './actions';
import CurrencyPairsFilter from './CurrencyPairsFilter';
import RateList from './RateList';

type Props = {
  configRequestStatus: RequestStatus,
  currencyPairs: List<CurrencyPair>,
  rates: List<CurrencyPairRate>,
  fetchConfig: () => void,
  fetchRates: (interval: number, ids: Array<string>) => void,
};

type State = {};

const Container = styled.div`
  height: 100vh;
  width: 100vw;
  padding: 1rem 2rem;
`;

const Heading = styled.h1`
  margin: 0 0 2rem;
  font-size: 2rem;
`;

const StyledFilter = styled(CurrencyPairsFilter)`
  margin: 0 0 1rem;
`;

const StyledRateList = styled(RateList)`
  margin: 0 0 1rem;
`;

class Home extends Component<Props, State> {
  static defaultProps = {};

  state = {};

  componentDidMount() {
    this.props.fetchConfig();
  }

  render() {
    const { configRequestStatus, currencyPairs, rates } = this.props;

    return (
      <Container>
        <Heading>Exchange Rate App</Heading>
        {configRequestStatus === 'success' ? (
          <>
            <StyledFilter currencyPairs={currencyPairs} />
            <StyledRateList rates={rates} />
          </>
        ) : (
          <div>Loading...</div>
        )}
      </Container>
    );
  }
}

const mapStateToProps = state => ({
  configRequestStatus: state.getIn(['app', 'configRequestStatus']),
  currencyPairs: getCurrencyPairList(state),
  rates: getCurrencyPairRates(state),
});

const mapDispatchToProps = { fetchConfig, fetchRates };

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Home);
