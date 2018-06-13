import React from 'react';
import { map } from 'lodash/fp';

// map(() => {})(data)
// {"70c6744c-cba2-5f4c-8a06-0dac0c4e43a1":[{"code":"AMD","name":"Armenia Dram"},{"code":"GEL","name":"Georgia Lari"}],
// 

const CurrencyPairs = (props) => {
    const { currencyPairs } = props;

    return (
        <ul>{map((pair) => (
            <li></li>
        ))(currencyPairs)}     
            </ul>
    )
}

export default CurrencyPairs;