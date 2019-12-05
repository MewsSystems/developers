import React, { Component } from 'react';
import { connect } from 'react-redux';
import { values } from 'lodash';
import { setFilterValue, getFilterBy} from '../redux/reducers/rates';
import { ALL_CURRENCIES } from '../constants/rates'

class CurrencyPairSelector extends Component {

    handleChange = (event) => {
        this.props.setFilterValue(event.target.value);
    };

    render() {
        const { rates, filterBy } = this.props;
        const options = [{id: ALL_CURRENCIES, label: 'All'}, ...values(rates)];

        return <div>
            Select a currency pair
            <select value={filterBy} onChange={this.handleChange}>
                {
                    options.map(option => <option key={option.id} value={option.id}>{ option.label }</option>)
                }
            </select>
        </div>;
    }
}

const mapStateToProps = state => {
    return {
        filterBy: getFilterBy(state),
    };
};

const mapDispatchToProps = dispatch => ({
    setFilterValue: currencyId => dispatch(setFilterValue(currencyId))
});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(CurrencyPairSelector);