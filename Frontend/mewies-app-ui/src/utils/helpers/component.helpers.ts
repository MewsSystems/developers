export const generateID = (length: number) => {
    let result = ''
    const characters =
        'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789'
    const charactersLength = characters.length
    for (let i = 0; i < length; i++) {
        result += characters.charAt(
            Math.floor(Math.random() * charactersLength)
        )
    }
    return result
}

export const setMargin = (margin: Array<0 | 4 | 8 | 16 | 32>): string => {
    const [top, right, bottom, left] = margin
    return `margin: ${top ? top : 0}px ${right ? right : 0}px ${
        bottom ? bottom : 0
    }px ${left ? left : 0}px;`
}

export const formatReleaseDate = (date: string) => {
    return date.split('-')[0] ? date.split('-')[0] : ''
}
