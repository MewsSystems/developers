import React from 'react';
import {map} from 'lodash/fp';

// map(() => {})(data)
// {"70c6744c-cba2-5f4c-8a06-0dac0c4e43a1":[{"code":"AMD","name":"Armenia Dram"},{"code":"GEL","name":"Georgia Lari"}],
//

// currencyPairs: {
//   '5b428ac9-ec57-513d-8a08-20199469fb4d': [{code: "LSL", name: "Lesotho Loti"}, {code: "AUD", name: "Australia Dollar"}]
// }

// '5b428ac9-ec57-513d-8a08-20199469fb4d'

const CurrencyPairs = (props) => {
    const {currencyPairs} = props;

    return (
        <div>
            <ul>
                {currencyPairs.map(currency => (
                    <li key={currency.id}>{currency.currency1['name'] + ' (' + currency.currency1['code'] + ') : ' + currency.currency2['name'] + ' (' + currency.currency2['code'] + ')'}</li>
                ))}
            </ul>
        </div>
    );
};

export default CurrencyPairs;