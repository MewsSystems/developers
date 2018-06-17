import React from 'react';


// ["70c6744c-cba2-5f4c-8a06-0dac0c4e43a1", "41cae0fd-b74d-5304-a45c-ba000471eabd"]

const CurrencyPairs = (props) => {
    const {currencyPairs} = props;

    let arrayOfIds = [];
    currencyPairs.forEach(x => arrayOfIds.push(x['id']));

    const currencyLis = currencyPairs.map(currency => (
        <li key={currency.id}>{currency.currency1['code'] + ' / ' + currency.currency2['code']}</li>
    ));

    props.fetchCurrencyRates(arrayOfIds);

    return (
        <div>
            <ul>
                {/*{currencyPairs.map(currency => (*/}
                    {/*<li key={currency.id}>{currency.currency1['code'] + ' / ' + currency.currency2['code']}</li>*/}
                {/*))}*/}
                {currencyLis}
            </ul>
        </div>
    );
};

export default CurrencyPairs;