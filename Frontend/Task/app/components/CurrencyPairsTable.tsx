/**
 * Represent Active, Disabled & Locked Trusted Shares
 */
import React from 'react'

import { CurrencyPair } from '../store/types';

interface CurrencyPairsTableProps {
    currencyPairs: CurrencyPair[]
}

const CurrencyPairsTable: React.FC<CurrencyPairsTableProps> = ({ currencyPairs }: CurrencyPairsTableProps) => (
    <>
        <div className="Table">
            <div className="Table__Header">
                <span>Pair</span>
                <span>Value</span>
            </div>
            <div className="TableBody">
            {currencyPairs && currencyPairs.length > 0 &&
                    currencyPairs.map((pair: CurrencyPair, i: number) => {
                        return (
                            <div className="Table__Row" key={i}>
                                <span>{`${pair[0].code}/${pair[1].code}`}</span>
                                <span></span>
                            </div>
                        )
                    })
            }
            </div>
        </div>
    </>

);

CurrencyPairsTable.displayName = 'CurrencyPairsTable';


export default CurrencyPairsTable;