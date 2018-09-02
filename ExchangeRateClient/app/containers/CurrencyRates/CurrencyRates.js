import React from 'react';
import PropTypes from 'prop-types';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Col, Container, Row, UncontrolledAlert } from 'reactstrap';
import { applySpec, isEmpty, isNil, pluck } from 'ramda';
import { collectiveToggleTracking, toggleTracking, updateCurrentPage } from '../../actions';
import { Countdown, ErrorMessages, Table } from '../../components';
import {
    getCurrencyPairsAllIds, getErrorMessages, getInterval, getShowCountdown, getOrderedCurrencyPairs,
} from '../../selectors';
import { prepareTableBodyData } from './utils';
import Toolbar from './toolbar';

class CurrencyRates extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            displayRows: null,
            maxNumberOfRows: 5,
        };
    }

    static getDerivedStateFromProps(nextProps, prevState) {
        if (isNil(prevState.displayRows) && !isEmpty(nextProps.orderedCurrencyPairs)) {
            return { ...prevState, displayRows: pluck('id', nextProps.orderedCurrencyPairs) };
        }
        return null;
    }

    onHandleDisplayRows = (filteredIds) => this.setState({ displayRows: filteredIds });
    onHandleChangeMaxNumberOfRows = (event) => this.setState({ maxNumberOfRows: parseInt(event.target.value, 10) });

    render() {
        const { displayRows, maxNumberOfRows } = this.state;
        const {
            errorMessages, interval, orderedCurrencyPairs, showCountdown, toggleTracking, updateCurrentPage,
        } = this.props;
        const boundActions = { toggleTracking };
        return (
            <Container>
                <Row className="mb-3">
                    <Col>
                        <h1>Currency Rates</h1>
                    </Col>
                </Row>
                <Row className="mb-3">
                    <Col md="12">
                        <ErrorMessages errorMessages={errorMessages} timeout={interval - 1000} />
                    </Col>
                </Row>
                <Row className="mb-3">
                    <Col md="12">
                        <UncontrolledAlert color="primary">
                            <i className="fas fa-info-circle mr-2" />
                            Click on ID of currency pair to show rates history.
                        </UncontrolledAlert>
                    </Col>
                </Row>
                <Row className="mb-3">
                    <Toolbar
                        orderedCurrencyPairs={orderedCurrencyPairs}
                        displayRows={this.onHandleDisplayRows}
                        changeMaxNumberOfRows={this.onHandleChangeMaxNumberOfRows}
                        maxNumberOfRows={maxNumberOfRows}
                    />
                </Row>
                <Row>
                    <Col md="12">
                        <Table
                            maxNumberOfPageItems={8}
                            maxNumberOfRows={maxNumberOfRows}
                            tableHeader={['ID', 'Currency Pair', 'Current Rate', 'Trend', 'Growth Rate', 'Track?']}
                            tableBody={prepareTableBodyData(boundActions, orderedCurrencyPairs, displayRows)}
                            updateCurrentPage={updateCurrentPage}
                        />
                    </Col>
                </Row>
                <Row>
                    <Col md={4}>
                        {showCountdown && <Countdown seconds={interval / 1000} />}
                    </Col>
                </Row>
            </Container>
        );
    }
}

CurrencyRates.propTypes = {
    collectiveToggleTracking: PropTypes.func,
    errorMessages: PropTypes.any,
    interval: PropTypes.number,
    orderedCurrencyPairs: PropTypes.array,
    showCountdown: PropTypes.bool,
    toggleTracking: PropTypes.func,
    updateCurrentPage: PropTypes.func,
};

const mapStateToProps = applySpec({
    orderedCurrencyPairs: getOrderedCurrencyPairs,
    currencyPairsOrder: getCurrencyPairsAllIds,
    errorMessages: getErrorMessages,
    interval: getInterval,
    showCountdown: getShowCountdown,
});

const mapDispatchToProps = (dispatch) => bindActionCreators({
    collectiveToggleTracking,
    toggleTracking,
    updateCurrentPage: updateCurrentPage(['currencyRates']),
}, dispatch);

export default connect(mapStateToProps, mapDispatchToProps)(CurrencyRates);
