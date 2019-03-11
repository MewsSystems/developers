import PairInterface from './pairInterface';
import { MessageInterface } from './MessageInterface';

export interface ExchangeInterface {
    loading: boolean;
    error: boolean;
    pairs?: PairInterface;
    selectedPair?: string;
    message?: MessageInterface;
}
