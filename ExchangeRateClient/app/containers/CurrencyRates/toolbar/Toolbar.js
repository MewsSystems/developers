import React from 'react';
import PropTypes from 'prop-types';
import { Col } from 'reactstrap';
import FilterRowsForm from './FilterRowsForm';
import SelectNumberOfRows from './SelectNumberOfRows';
import SortToolbar from './SortToolbar';
import TrackToolbar from './TrackToolbar';

const Toolbar = ({ changeMaxNumberOfRows, orderedCurrencyPairs, displayRows, maxNumberOfRows }) => (
    <React.Fragment>
        <Col className="text-left" md="2">
            <FilterRowsForm
                orderedCurrencyPairs={orderedCurrencyPairs}
                handleChange={displayRows}
            />
        </Col>
        <Col className="text-right" md="6">
            <SortToolbar />
        </Col>
        <Col className="text-center" md="3">
            <TrackToolbar
                orderedCurrencyPairs={orderedCurrencyPairs}
                maxNumberOfRows={maxNumberOfRows}
            />
        </Col>
        <Col className="text-right" md="1">
            <SelectNumberOfRows
                handleChange={changeMaxNumberOfRows}
            />
        </Col>
    </React.Fragment>
);

Toolbar.propTypes = {
    changeMaxNumberOfRows: PropTypes.func,
    displayRows: PropTypes.func,
    maxNumberOfRows: PropTypes.number,
    numberOfCurrencyPairs: PropTypes.number,
    orderedCurrencyPairs: PropTypes.array,
};

export default Toolbar;
