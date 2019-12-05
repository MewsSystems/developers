import React, { Component, Fragment } from 'react';
import { connect } from 'react-redux';
import { values } from 'lodash';
import {
    initializeConfigurationList,
    getList,
    getLoading,
    getLoaded,
    getError
} from '../redux/reducers/rates';
import CurrencyPairSelector from './CurrencyPairSelector';

class RateList extends Component {

    componentDidMount() {
        this.props.initializeConfigurationList();
    }

    render() {
        const { rates, loaded, loading, error } = this.props;

        if (loading) {
            return (
                <h2>Loading</h2>
            );
        } else {
            if ( values(rates).length === 0 && loaded) {
                return <h3>No currency pairs to display, sorry!</h3>
            }

            return (
                <Fragment>
                    <CurrencyPairSelector rates={rates} />
                    { error.rates && <h2>Data update failed. Wait for a next attempt...</h2> }
                    <table>
                        <thead>
                        <tr>
                            <th>Currency pair</th>
                            <th>Rate</th>
                            <th>Trend</th>
                        </tr>
                        </thead>
                        <tbody>
                        {
                            values(rates).map(item => {
                                return (
                                    <tr key={item.id}>
                                        <td>{ item.label }</td>
                                        <td>{ item.rate } </td>
                                        <td>{ item.trend }</td>
                                    </tr>)
                            })
                        }
                        </tbody>
                    </table>
                </Fragment>
            );

        }
    }
}



const mapStateToProps = state => {
    return {
        rates: getList(state),
        loading: getLoading(state),
        loaded: getLoaded(state),
        error: getError(state)
    };
};

const mapDispatchToProps = dispatch => ({
    initializeConfigurationList: () => dispatch(initializeConfigurationList())
});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(RateList);