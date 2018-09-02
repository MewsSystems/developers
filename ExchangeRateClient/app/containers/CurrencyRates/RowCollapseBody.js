import React from 'react';
import PropTypes from 'prop-types';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Col, Container, ListGroup, ListGroupItem, ListGroupItemHeading, ListGroupItemText, Row } from 'reactstrap';
import { updateCurrentPage } from '../../actions';
import { Table } from '../../components';
import { getHighestRateFromCurrencyPair, getLowestRateFromCurrencyPair, getRateHistoryEntries } from '../../selectors';

const prepareData = ({ timestamp, rate }) => ({
    rowData: [{ cellType: 'display', cellData: timestamp }, { cellType: 'display', cellData: rate }],
});

const RowCollapseBody = ({ id, pair, highestRate, lowestRate, rateHistoryEntries, updateCurrentPage }) =>
    <Container>
        <Row>
            <Col md="6">
                <h2 className="mb-4">Rate history</h2>
                <p><b>ID: </b>{id}</p>
                <p><b>Currency pair: </b><br />{pair}</p>
                <ListGroup>
                    <ListGroupItem>
                        <ListGroupItemHeading>Highest rate</ListGroupItemHeading>
                        <ListGroupItemText>
                            <b>{highestRate.rate}</b> on {highestRate.timestamp}
                        </ListGroupItemText>
                    </ListGroupItem>
                    <ListGroupItem>
                        <ListGroupItemHeading>Lowest rate</ListGroupItemHeading>
                        <ListGroupItemText>
                            <b>{lowestRate.rate}</b> on {lowestRate.timestamp}
                        </ListGroupItemText>
                    </ListGroupItem>
                </ListGroup>
            </Col>
            <Col md="6">
                <Table
                    borderless
                    maxNumberOfPageItems={8}
                    maxNumberOfRows={10}
                    size="sm"
                    tableHeader={['Timestamp', 'Rate']}
                    tableBody={rateHistoryEntries.map(prepareData)}
                    updateCurrentPage={updateCurrentPage}
                />
            </Col>
        </Row>
    </Container>;

RowCollapseBody.propTypes = {
    highestRate: PropTypes.oneOfType([
        PropTypes.shape({
            rate: PropTypes.number,
            timestamp: PropTypes.string,
        }),
        PropTypes.string,
    ]),
    id: PropTypes.string,
    lowestRate: PropTypes.oneOfType([
        PropTypes.shape({
            rate: PropTypes.number,
            timestamp: PropTypes.string,
        }),
        PropTypes.string,
    ]),
    pair: PropTypes.string,
    rateHistoryEntries: PropTypes.array.isRequired,
    updateCurrentPage: PropTypes.func,
};

const mapStateToProps = (state, { id }) => ({
    highestRate: getHighestRateFromCurrencyPair(id)(state),
    lowestRate: getLowestRateFromCurrencyPair(id)(state),
    rateHistoryEntries: getRateHistoryEntries(id)(state),
});

const mapDispatchToProps = (dispatch, { id }) => bindActionCreators({
    updateCurrentPage: updateCurrentPage(['ratesHistory', id]),
}, dispatch);

export default connect(mapStateToProps, mapDispatchToProps)(RowCollapseBody);
