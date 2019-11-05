import React from 'react';
import { reduxForm } from 'redux-form';
import { Field } from 'redux-form';
import CheckboxGroup from '../CheckboxGroup';
import styles from './CurrencyPairsSelector.module.css';

const buildCurrencyPairsSelectorOptions = (currencyPairs = {}) => {

    const options = [];

    for (let key in currencyPairs) {
        options.push({
            value: key,
            label: `${currencyPairs[key][0].code} / ${currencyPairs[key][1].code} - ${currencyPairs[key][0].name} / ${currencyPairs[key][1].name}`
        });
    }

    return options;
};

let LoanForm = ({handleSubmit, currencyPairs = {}}) => {

    const currencyPairsSelectorOptions = buildCurrencyPairsSelectorOptions(currencyPairs);

    return (
        <div
            className={styles['container']}
        >
            <div
                className={styles['form-header']}
            >
                Select Currency Pairs
            </div>
            <form
                onSubmit={handleSubmit}
            >
                <Field
                    type="checkbox"
                    component={CheckboxGroup}
                    name="selectedCurrencyPairsIds"
                    options={currencyPairsSelectorOptions}
                />
            </form>
        </div>
    );
};

export default reduxForm({
    // a unique name for the form
    form: 'currencyPairsSelector',
    initialValues: {
        selectedCurrencyPairsIds: [],
    }
})(LoanForm);
