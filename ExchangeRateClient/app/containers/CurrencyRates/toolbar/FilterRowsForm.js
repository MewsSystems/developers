import React from 'react';
import PropTypes from 'prop-types';
import { assoc, compose, contains, filter, isEmpty, keys, toLower, prop, reduceRight, isNil } from 'ramda';
import { Form, FormGroup, Input } from 'reactstrap';
import { currencyPairToString } from '../../../utils';

class FilterRowsForm extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            currencyPairsNamesTable: null,
        };
    }

    static getDerivedStateFromProps(nextProps, prevState) {
        if (isNil(prevState.currencyPairsNamesTable) && !isEmpty(nextProps.orderedCurrencyPairs)) {
            return {
                ...prevState,
                currencyPairsNamesTable: reduceRight(
                    ({ id, pair }, accumulator) => assoc(id, toLower(currencyPairToString(pair)), accumulator),
                    {},
                    nextProps.orderedCurrencyPairs
                ),
            };
        }
        return null;
    }

    handleOnChange = (event) => {
        const filteredIds = compose(
            keys,
            filter(contains(toLower(event.target.value))),
            prop('currencyPairsNamesTable'),
        )(this.state);
        this.props.handleChange(filteredIds);
    };

    render() {
        return (
            <Form inline>
                <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
                    <Input
                        type="text"
                        placeholder="Filter rows..."
                        onChange={this.handleOnChange}
                    />
                </FormGroup>
            </Form>
        );
    }
}

FilterRowsForm.propTypes = {
    handleChange: PropTypes.func,
    orderedCurrencyPairs: PropTypes.array,
};

export default FilterRowsForm;
