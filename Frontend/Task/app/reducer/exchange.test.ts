import ExchangeReducer from './exchange';
import { ExchangeAction } from '../interface/exchangeActionInterface';
import pairsFixture from '../fixture/pairsFixture';
import {MessageType} from '../interface/MessageInterface';

const initialStateFixture = { loading: false, error: false };

describe('Exchange reducer test', () => {
    it('should have initial state', () => {
        expect(
            ExchangeReducer(initialStateFixture, { type: ExchangeAction.GET_PAIRS, payload: {}}),
        ).toEqual({
            loading: false,
            error: false,
            pairs: {},
        });
    });

    it('should have currency pairs', () => {
        expect(ExchangeReducer(
            initialStateFixture,
            { type: ExchangeAction.GET_PAIRS, payload: pairsFixture },
        )).toEqual({
            loading: false,
            error: false,
            pairs: pairsFixture,
        });
    });

    it('should toggle error', () => {
        expect(ExchangeReducer(
            initialStateFixture,
            { type: ExchangeAction.ERROR_TOGGLE },
        )).toEqual({
            loading: false,
            error: true,
        });

        expect(ExchangeReducer(
            initialStateFixture,
            { type: ExchangeAction.ERROR_TOGGLE, payload: false },
        )).toEqual({
            loading: false,
            error: false,
        });
    });

    it('should toggle loading', () => {
        expect(ExchangeReducer(
            initialStateFixture,
            { type: ExchangeAction.LOADING_TOGGLE },
        )).toEqual({
            loading: true,
            error: false,
        });

        expect(ExchangeReducer(
            initialStateFixture,
            { type: ExchangeAction.LOADING_TOGGLE, payload: false },
        )).toEqual({
            loading: false,
            error: false,
        });
    });

    it('should add a message', () => {
        expect(ExchangeReducer(
            initialStateFixture,
            {
                type: ExchangeAction.ADD_MESSAGE,
                payload: {
                    message: 'some message',
                    type: MessageType.ERROR,
                },
            },
        )).toEqual({
            loading: false,
            error: false,
            message: {
                message: 'some message',
                type: MessageType.ERROR,
            },
        });
    });
});
