import React from 'react'

import { KeyByCurrencyPair } from '../store/types';
import TableRow from './TableRow';

interface CurrencyPairsTableProps {
    currencyPairs: KeyByCurrencyPair[],
    currencyPairsIdList: string[]
}

const CurrencyPairsTable: React.FC<CurrencyPairsTableProps> = ({ currencyPairs, currencyPairsIdList }: CurrencyPairsTableProps) => (
    <>
        <div className="Table">
            <div className="Table__Header">
                <span>Pair</span>
                <span>Value</span>
            </div>
            <div className="TableBody">
            {currencyPairsIdList && currencyPairsIdList.length > 0 &&
                    currencyPairsIdList.map((id: string) => {
                        return (
                            <TableRow key={id} id={id} currencyPair={currencyPairs[id as any] as any} />
                        )
                    })
            }
            </div>
        </div>
    </>

);

CurrencyPairsTable.displayName = 'CurrencyPairsTable';

export default CurrencyPairsTable;