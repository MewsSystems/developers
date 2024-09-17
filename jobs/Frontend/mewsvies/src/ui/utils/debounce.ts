export const debounce = (callback: (...args: any[]) => void, wait: number) => {
    let timeoutId: NodeJS.Timeout

    return (...args: any[]) => {
        clearTimeout(timeoutId)
        timeoutId = setTimeout(() => callback(...args), wait)
    }
}
