import React, { ReactElement } from 'react';
import { iteratePairCollection } from '../action/currencyPair';
import PairInterface from '../interface/pairInterface';
import CurrencyPairInterface from '../interface/currencyPairInterface';
import PairSelectorStyle from '../style/pairSelector';

const getOption = (
    key: string, currencyPair: CurrencyPairInterface,
): ReactElement<any> => {
    const {shortcut} = currencyPair;
    return (
        <option value={key} key={key}>{shortcut}</option>
    );
};


const CurrencyPairSelector: React.FC<{ pairs: PairInterface, selectedPair: string, changeAction: (pairId: string) => any }> = (
    {pairs, selectedPair, changeAction},
) => {
    return (
        <PairSelectorStyle>
            <label>Filter:</label>
            <select defaultValue={selectedPair} onChange={(event: any) => changeAction(event.target.value)}>
                <option value="">--</option>
                {pairs && iteratePairCollection(pairs, getOption)}
            </select>
        </PairSelectorStyle>
    );
};

export default CurrencyPairSelector;
