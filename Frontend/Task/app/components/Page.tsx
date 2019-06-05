import React from "react";
import { Exchange } from "./Exchange";
import {
  AppState,
  CurrencyPairIds,
  ExchangeThunkDispatch
} from "../store/types";
import { bindActionCreators } from "redux";
import { fetchExhangeRates, downloadConfiguration } from "../store/actions";
import { connect } from "react-redux";
import * as config from "../config.json";
import { Title, Container } from "./styled";

type StateProps = ReturnType<typeof mapStateToProps>;
type DispatchProps = ReturnType<typeof mapDispatchToProps>;
type PageProps = DispatchProps & StateProps;

export class PageComponent extends React.Component<PageProps> {
  public componentDidMount(): void {
     this.getExchangeRates();
  }

  private getExchangeRates = () => {
    const {
      fetchExhangeRates,
      currencyPairs,
      downloadConfiguration
    } = this.props;

    if (currencyPairs) {
      const allCurrencyRates: CurrencyPairIds = {
        currencyPairIds: Object.keys(currencyPairs)
      };
      fetchExhangeRates(allCurrencyRates);
    } else {
      downloadConfiguration();
    }
  };

  public render() {
    return (
      <Container>
        <Title>Exchange rate list</Title>
        <Exchange
          interval={config.interval}
          onNextUpdate={this.getExchangeRates}
        />
      </Container>
    );
  }
}

const mapStateToProps = (state: AppState) => ({
  currencyPairs: state.currencyPairs
});

const mapDispatchToProps = (dispatch: ExchangeThunkDispatch) =>
  bindActionCreators(
    {
      fetchExhangeRates,
      downloadConfiguration
    },
    dispatch
  );

export const Page = connect<StateProps, DispatchProps>(
  mapStateToProps,
  mapDispatchToProps
)(PageComponent);
