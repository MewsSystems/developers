import React from 'react';
import PropTypes from 'prop-types';
import { Button, Form, FormFeedback, FormGroup, Input, Table as BSTable } from 'reactstrap';
import { and, compose, converge, inc, not } from 'ramda';
import { inRange, isNilOrEmpty, mapIndexed, notBoth } from 'ramda-adjunct';
import TableRow from './TableRow';
import Pagination from './Pagination';
import { sliceForPagination } from './utils';
import { stringIsPositiveInteger } from '../../validations/number';

const renderTableHeader = (data) => <tr>{data.map((key) => <th key={key}>{key}</th>)}</tr>;

// eslint-disable-next-line react/prop-types
const renderTableRows = ({ id, rowData, collapseBody }, index) =>
    <TableRow
        key={isNilOrEmpty(id) ? index : id}
        id={id}
        index={index}
        rowData={rowData}
        collapseBody={collapseBody}
    />;

class Table extends React.Component {

    state = {
        currentPage: 1,
        goToPage: '',
        paginationErrorFeedback: '',
    };

    handleGoToPageChange = (event) => {
        const goToPage = event.target.value;
        const numberOfPages = Math.ceil(this.props.tableBody.length / this.props.maxNumberOfRows);
        if (converge(and, [
            notBoth(stringIsPositiveInteger, inRange(1, inc(numberOfPages))),
            compose(not, isNilOrEmpty),
        ])(goToPage)) {
            this.setState({ paginationErrorFeedback: `Please enter a positive integer between 1 and ${numberOfPages}.` });
        } else {
            this.setState({ goToPage, paginationErrorFeedback: '' });
        }
    };

    handleGoToPageKeyDown = (event) => {
        if (event.keyCode === 13) {
            event.preventDefault();
            this.onHandlePageChange(parseInt(this.state.goToPage, 10))();
        }
    };

    handleGoToPageClick = () => {
        this.onHandlePageChange(parseInt(this.state.goToPage, 10))();
    };

    onHandlePageChange = (newPage) => () => {
        if (isNilOrEmpty(this.state.paginationErrorFeedback) && (!isNilOrEmpty(newPage) && !Number.isNaN(newPage))) {
            this.props.updateCurrentPage(newPage);
            this.setState({ currentPage: newPage, goToPage: '' });
        }
    };

    render() {
        const { currentPage, paginationErrorFeedback } = this.state;
        const { borderless, maxNumberOfPageItems, maxNumberOfRows, size, tableHeader, tableBody } = this.props;
        const numberOfPages = Math.ceil(tableBody.length / maxNumberOfRows);
        return (
            <React.Fragment>
                <BSTable borderless={borderless} size={size}>
                    <thead>{renderTableHeader(tableHeader)}</thead>
                    <tbody>
                        {compose(
                            mapIndexed(renderTableRows),
                            sliceForPagination(currentPage, maxNumberOfRows)
                        )(tableBody)}
                        </tbody>
                </BSTable>
                { tableBody.length > maxNumberOfRows &&
                    <React.Fragment>
                        <Form className="mb-3" inline>
                            <FormGroup>
                                <Input
                                    type="text"
                                    placeholder={`Go to page... 1-${numberOfPages}`}
                                    onChange={this.handleGoToPageChange}
                                    onKeyDown={this.handleGoToPageKeyDown}
                                    invalid={!isNilOrEmpty(paginationErrorFeedback)}
                                    className="mr-2"
                                />
                                <Button onClick={this.handleGoToPageClick}><i className="fas fa-arrow-right" /></Button>
                                { !isNilOrEmpty(paginationErrorFeedback) &&
                                    <FormFeedback>{paginationErrorFeedback}</FormFeedback>
                                }
                            </FormGroup>
                        </Form>
                        <Pagination
                            currentPage={currentPage}
                            numberOfPages={numberOfPages}
                            maxNumberOfPageItems={
                                numberOfPages < maxNumberOfPageItems ? numberOfPages : maxNumberOfPageItems
                            }
                            handlePageChange={this.onHandlePageChange}
                        />
                    </React.Fragment>
                }
            </React.Fragment>
        );
    }
}
Table.propTypes = {
    borderless: PropTypes.bool,
    maxNumberOfPageItems: PropTypes.number,
    maxNumberOfRows: PropTypes.number,
    size: PropTypes.string,
    tableBody: PropTypes.arrayOf(PropTypes.shape({
        id: PropTypes.string,
        rowData: PropTypes.arrayOf(PropTypes.shape({
            cellType: PropTypes.string,
            cellData: PropTypes.any,
        })).isRequired,
    })).isRequired,
    tableHeader: PropTypes.arrayOf(PropTypes.string).isRequired,
    updateCurrentPage: PropTypes.func,
};

export default Table;
