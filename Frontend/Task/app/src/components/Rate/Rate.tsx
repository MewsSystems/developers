import React from 'react';
import styled from 'styled-components'
import * as skin from './Rate.skin'
import {connect} from 'react-redux';

import PairName from "../PairName";
import Preloader from '../Preloader'

const RateWrapper = styled.div`${skin.RateWrapper}`;
const RateValues = styled.div`${skin.RateValues}`;
const RatesLoading = styled(Preloader)`${skin.RatesLoading}`;
const RatesPairName = styled(PairName)`${skin.RatePairName}`;


type RateProps = {
    id: string,
    pair: Array<{
        code: string,
        name: string
    }>
    rates: {
        value: number,
        trend: string
    }
    loading: boolean
}

const Rate: React.FC<RateProps> = (props) => {
    const { rates, pair, loading } = props;
    let Trend;

    switch(rates.trend) {
        case "growing":
            Trend = styled.span`${skin.Growing}`;
            break;
        case "falling":
            Trend = styled.span`${skin.Falling}`;
            break;
        default:
            Trend = styled.span`${skin.Arrow}`;
            break;
    }

    return (
        <RateWrapper>
            <RatesPairName>
                {pair}
            </RatesPairName>
            {loading && (
                <RatesLoading/>
            )}
            {!loading && (
                <RateValues>
                    {rates.value}
                    <Trend />
                </RateValues>
            )}
        </RateWrapper>
    );
};

const mapStateToProps = state => {
    return {
        loading: state.ratesLoading,
    };
};


export default connect(mapStateToProps)(Rate);