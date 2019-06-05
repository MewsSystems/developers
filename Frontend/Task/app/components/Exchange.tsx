import * as React from "react";
import { WithUpdater, WithUpdaterProps } from "./HOC/WithUpdater";
import { connect } from "react-redux";
import { compose } from "redux";
import { AppState, Rates } from "../store/types";
import _isEqual from "lodash/isEqual";
import { ExchangeRateItem, ExchangeRateItemDTO } from "./ExchangeRateItem";
import { ExchangeRateTitles } from "./ExchangeRateTitles";
import { ExchangeWrapper, List, ListItem } from "./styled";
import { InputText } from "./InputText";
import { Loader } from "./Loader";

export enum TrendEnum {
  growing = "growing",
  declining = "declining",
  stagnating = "stagnating"
}

interface ExchangeState {
  rates: ExchangeRateItemDTO[];
  filteredValue?: ExchangeRateItemDTO;
}

type StateProps = ReturnType<typeof mapStateToProps>;

type ExchangeProps = StateProps;

class ExchangeComponent extends React.Component<ExchangeProps, ExchangeState> {
  public state = {
    rates: [],
    filteredValue: undefined
  };

  public componentWillReceiveProps(nextProps: Readonly<StateProps>): void {
    if (!_isEqual(nextProps.rates, this.props.rates)) {
      this.props.rates &&
        nextProps.rates &&
        this.updateRatesData(this.props.rates, nextProps.rates);
    }
  }

  private handleTrend = (prevValue: number, currentValue: number) => {
    if (prevValue < currentValue) {
      return TrendEnum.growing;
    } else if (prevValue > currentValue) {
      return TrendEnum.declining;
    } else {
      return TrendEnum.stagnating;
    }
  };

  private updateRatesData = (rates: Rates, nextRates: Rates) => {
    const { currencyPairs } = this.props;
    const dataWithTrend = Object.entries(nextRates).map(
      ([key, value]): ExchangeRateItemDTO => ({
        id: key,
        trend: this.handleTrend(rates[key], nextRates[key]),
        value,
        currencyPair: currencyPairs && currencyPairs[key]
      })
    );
    this.setState({ rates: dataWithTrend });
  };

  private findRate = (e: React.FormEvent<HTMLInputElement>) => {
    const value = e.currentTarget.value;
    const result = this.state.rates.find(
      (rate: ExchangeRateItemDTO) =>
        rate.currencyPair[0].name === value ||
        rate.currencyPair[0].code === value ||
        rate.currencyPair[1].name === value ||
        rate.currencyPair[1].code === value
    );
    if (result) {
      this.setState({ filteredValue: result });
    } else {
      this.setState({ filteredValue: undefined });
    }
  };

  private renderRates = (rates: ExchangeRateItemDTO[]) => (
    <>
      {rates.map((rate: ExchangeRateItemDTO) => (
        <ListItem key={rate.id} withBorder>
          <ExchangeRateItem rate={rate} />
        </ListItem>
      ))}
    </>
  );

  private renderFilteredValue = (filteredValue: ExchangeRateItemDTO) => (
    <ListItem withBorder>
      <ExchangeRateItem rate={filteredValue} />
    </ListItem>
  );

  public render() {
    const { filteredValue, rates } = this.state;
    const { isFetching } = this.props;
    const hasRates = rates.length > 0;
    const showLoader = isFetching || !hasRates;
    return (
      <ExchangeWrapper>
        <InputText onChange={this.findRate} placeholder="find currency" />
        <List>
          <ListItem>
            <ExchangeRateTitles />
          </ListItem>
          {filteredValue
            ? this.renderFilteredValue(filteredValue as any) // already asking TS if it exits
            : this.renderRates(rates)}
        </List>
        {showLoader && <Loader />}
      </ExchangeWrapper>
    );
  }
}

const mapStateToProps = (state: AppState) => ({
  rates: state.rates,
  currencyPairs: state.currencyPairs,
  isFetching: state.isFetching
});

export const Exchange = compose<React.StatelessComponent<WithUpdaterProps>>(
  connect<StateProps, null, WithUpdaterProps>(
    mapStateToProps,
    null
  ),
  WithUpdater
)(ExchangeComponent);
