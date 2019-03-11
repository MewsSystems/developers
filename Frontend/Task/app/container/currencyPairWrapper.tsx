import React, { ReactElement, useEffect } from 'react';
import { connect } from 'react-redux';
import CurrencyPairSelector from '../component/currencyPairSelector';
import CurrencyPairList from '../component/currencyPairList';
import PairInterface from '../interface/pairInterface';
import setSelectedPair from '../action/setSelectedPair';
import { ExchangeInterface } from '../interface/exchangeInterface';
import getCurrencyPairs from '../action/getCurrencyPairs';
import RetryButton from '../component/retry';
import Loading from '../component/loading';
import getRates from '../action/getRates';
import config from '../config';
import Message from '../component/message';
import { MessageInterface } from '../interface/MessageInterface';

const CurrencyPairWrapper: React.FC<{
    pairs: PairInterface,
    selectedPair: string,
    changeAction: () => any,
    getPairs: () => any,
    rateRefresh: () => any
    loading: boolean,
    error: boolean,
    message: MessageInterface,
}> = (
    { pairs, selectedPair, changeAction, getPairs, rateRefresh, loading, error, message },
): ReactElement<any> => {
    useEffect(() => {
        if (!pairs) {
            getPairs();
        }
    }, [pairs]);

    useEffect(() => {
        const intervalId = setInterval(() => rateRefresh(), config.interval);

        return () => {
            clearInterval(intervalId);
        };
    });

    return (
        <>
            {error && <RetryButton reloadAction={getPairs}/>}
            {loading && <Loading />}
            {message && <Message message={message.message} type={message.type} />}
            {pairs &&
                <>
                    <CurrencyPairSelector
                        pairs={pairs}
                        selectedPair={selectedPair}
                        changeAction={changeAction}
                    />
                    <CurrencyPairList pairs={pairs} selectedPair={selectedPair}/>
                </>
            }
        </>
    );
};

const mapStateToProps = (state: ExchangeInterface) => ({
    pairs: state.pairs,
    selectedPair: state.selectedPair,
    loading: state.loading,
    error: state.error,
    message: state.message,
});

const mapDispatchToProps = {
    changeAction: setSelectedPair,
    getPairs: getCurrencyPairs,
    rateRefresh: getRates,
};

export default connect(mapStateToProps, mapDispatchToProps)(CurrencyPairWrapper);
