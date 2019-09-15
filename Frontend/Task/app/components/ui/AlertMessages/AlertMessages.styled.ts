import styled from 'styled-components';

export const AlertContainer = styled.div`
    position: fixed;
    z-index: 10;
    bottom: auto;
    bottom: 0;
    left: auto;
    width: 100%;
    max-width: 300px;
    font-size: 14px;
    padding: 15px 0;
    background-color: #444;
    color: #fff;
    border-radius: 2px;
    display: flex;
    align-items: flex-start;
    justify-content: flex-start;
    transform: translate3d(0, 100px, 0);
    transition: transform ease-in-out 0.2s;

    &.is-visible {
        transform: translate3d(0, -20px, 0);
    }
`

export const AlertMessage = styled.div`
    width: 255px;
    margin-left: 15px;
`

export const AlertCloseButton = styled.div`
    position: absolute;
    top: 6px;
    right: 0;
    width: 35px;
    height: 35px;
    line-height: 35px;
    color: #ccc;
    text-align: center;
    transition: color 0.1s;
    cursor: pointer;

    &:hover {
        color: #fff;
    }

    .Icon {
        font-size: 18px;
    }
`