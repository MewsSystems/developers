export const RateWrapper = `
    display: flex;
    height: 32px;
    padding: 1rem 0;
    justify-content: center;
    align-items: center;
    border-bottom: 1px solid #ccc
`;

export const RatePairName = `
    min-width: 5rem;
`;

export const RateValues = `
    display: flex;
    min-width: 8rem;
    align-items: center;
`;

export const RatesLoading = `
    min-width: 8rem;
    text-align: center;
    img {
        width: 2rem;
    }
`;

export const Arrow = `
    display: inline-block;
    margin-left: 0.5rem
    position: relative;
    width: 25px;
    height: 5px
    background: #bbb;
    &:before,&:after {
        display: block;
        position: absolute;
        content: '';
        right: -2px;
        top: 4px;
        height: 5px;
        height: 5px;
        width: 15px;
        background: #bbb;
        transform: rotate(-45deg);
    }
    &:after {
        top: -4px;
        transform: rotate(45deg);
    }
`;

export const Growing = `
    ${Arrow} {
        background: #73b504;
        transform: rotate(-90deg);
        &:before, &:after {
            background: #73b504;
        }
    }
`

export const Falling = `
    ${Arrow} {
        background: #ed1c24;
        transform: rotate(90deg);
        &:before, &:after {
            background: #ed1c24;
        }
    }
`