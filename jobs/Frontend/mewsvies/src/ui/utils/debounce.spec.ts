import { debounce } from './debounce.ts'

jest.useFakeTimers()
const callbackFunction = jest.fn()

describe('debounce', () => {
    it('should debounce the callback', () => {
        const debouncedCallbackFunction = debounce(callbackFunction, 1000)

        debouncedCallbackFunction()
        expect(callbackFunction).not.toHaveBeenCalled()

        jest.advanceTimersByTime(1000)
        expect(callbackFunction).toHaveBeenCalled()
    })
})
