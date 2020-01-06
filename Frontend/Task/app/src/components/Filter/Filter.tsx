import React from 'react'
import { connect } from 'react-redux';
import { update_filters } from '../../store/actions/currencyPairsRates.actions'
import styled from 'styled-components'
import * as skin from './Filter.skin'

import PairName from "../PairName";

type FilterProps = {
    id: string
    pairs: Array<{
        code: string,
        name: string,
    }>
    filters: string[]
    update_filters: Function
}

const FilterElement = styled.label`${skin.FilterElement}`;
const FilterCheckbox = styled.input`${skin.FilterCheckbox}`;
const FilterPairName = styled(PairName)`${skin.FilterPairName}`;

class Filter extends React.Component<FilterProps> {
    constructor(props) {
        super(props);
        this.toggleFilter = this.toggleFilter.bind(this);
    }

    public toggleFilter() {
        const { id } = this.props;
        this.props.update_filters(id);
    }

    render() {
        const { pairs, filters, id } = this.props;

        return (
            <FilterElement>
                <FilterCheckbox type="checkbox" onChange={this.toggleFilter} defaultChecked={filters.includes(id)}/>
                <FilterPairName>
                    { pairs }
                </FilterPairName>
            </FilterElement>
        );
    }
}

const mapStateToProps = state => {
    return {
        filters: state.filters
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        update_filters: (id: string) => dispatch(update_filters(id))
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Filter);
