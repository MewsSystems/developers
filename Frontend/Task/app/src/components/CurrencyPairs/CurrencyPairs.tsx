import React from 'react';
import {connect} from 'react-redux';
import styled from 'styled-components'
import * as skin from './CurrencyPairs.skin'

import Filter from "../Filter";
import Rate from "../Rate";

const ContentWrapper = styled.div`${skin.ContentWrapper}`;
const FilterWrapper = styled.div`${skin.FilterWrapper}`;
const RatesWrapper = styled.div`${skin.RatesWrapper}`;

type CurrencyPairsProps = {
    state: {
        config: Object,
        rates: {
            value: number,
            trend: string,
        }
        filters: string[]
    }
}

const CurrencyPairs: React.FC<CurrencyPairsProps> = (props) => {
    const {config, rates, filters} = props.state;

    return (
        <ContentWrapper>
            <FilterWrapper>
                {
                    Object.keys(config).map(key =>
                        <Filter key={key} id={key} pairs={config[key]}/>
                    )
                }
            </FilterWrapper>
            <RatesWrapper>
                {
                    Object.keys(rates).map(key => {
                            if(filters.includes(key) || filters.length < 1)
                                return <Rate key={key} id={key} rates={rates[key]} pair={config[key]} />;
                            return null
                        }
                    )
                }
            </RatesWrapper>
        </ContentWrapper>
    );
};

const mapStateToProps = state => {
    return {
        state
    }
};

export default connect(mapStateToProps)(CurrencyPairs);