// @flow
import React, { PureComponent } from 'react';
import ExchangeList from 'Components/exchangeList';
import ExchangeCardContainer from 'Containers/exchangeCard';
import Loader from 'Components/loader';

type Props = {
  currencyPairs: Object,
  currencyRates: Object,
  filteredIdList: any,
};

class ExchangeListContainer extends PureComponent<Props> {
  filterExchangeCards = (filteredIdList: any) => {
    const { currencyPairs, currencyRates } = this.props;

    if (
      Object.keys(currencyPairs).length !== 0 &&
      currencyPairs.constructor === Object
    ) {
      return Object.keys(filteredIdList).map(key => {
        const pairKey = filteredIdList[key];
        const pairs = currencyPairs[pairKey];
        const pairInfo = {
          first: {
            name: pairs[0].name,
            code: pairs[0].code,
          },
          second: {
            name: pairs[1].name,
            code: pairs[1].code,
          },
          rate: currencyRates[pairKey],
          pairKey,
        };
        return <ExchangeCardContainer key={pairKey} pairInfo={pairInfo} />;
      });
    }
    return null;
  };

  render() {
    const { filteredIdList } = this.props;
    if (filteredIdList.length < 1) {
      return <Loader />;
    }
    return (
      <ExchangeList>{this.filterExchangeCards(filteredIdList)}</ExchangeList>
    );
  }
}

export default ExchangeListContainer;
