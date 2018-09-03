// @flow
import React, { Component } from 'react';
import { List, Set } from 'immutable';
import { connect } from 'react-redux';
import styled from 'styled-components';

import type { CurrencyPair } from './reducer';
import { getCurrencyPairList } from './selectors';
import { fetchRates } from './actions';
import { interval } from './config';

type Props = {
  className: string,
  currencyPairs: List<CurrencyPair>,
  fetchRates: (interval: string, ids: Array<string>) => void,
};

type State = {
  selectedPairIds: Set<any>,
};

const ListWrapper = styled.ul`
  margin: 0;
  padding: 0;
`;

const ListItemWrapper = styled.li`
  margin: 0;
  padding: 0;
  list-style: none outside none;
`;

class CurrencyPairsFilter extends Component<Props, State> {
  static defaultProps = {
    className: '',
  };

  state = {
    selectedPairIds: Set(),
  };

  componentDidUpdate(prevProps, prevState) {
    if (this.state.selectedPairIds !== prevState.selectedPairIds) {
      this.props.fetchRates(interval, this.state.selectedPairIds.toArray());
    }
  }

  handleFilterChange = (e: SyntheticInputEvent<HTMLInputElement>) => {
    const { selectedPairIds } = this.state;
    const id = e.target.value;

    if (selectedPairIds.includes(id)) {
      this.setState({
        selectedPairIds: selectedPairIds.delete(id),
      });
    } else {
      this.setState({
        selectedPairIds: selectedPairIds.add(id),
      });
    }
  };

  render() {
    const { className, currencyPairs } = this.props;

    return (
      <ListWrapper className={className}>
        {currencyPairs.map(({ id, left, right }) => (
          <ListItemWrapper key={id}>
            <label>
              <input
                type="checkbox"
                value={id}
                onChange={this.handleFilterChange}
              />
              {left.name} / {right.name}
            </label>
          </ListItemWrapper>
        ))}
      </ListWrapper>
    );
  }
}

const mapStateToProps = state => ({
  currencyPairs: getCurrencyPairList(state),
});

const mapDispatchToProps = { fetchRates };

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(CurrencyPairsFilter);
