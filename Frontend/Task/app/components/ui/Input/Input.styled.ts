import styled from 'styled-components';
import Icon from '@components/ui/Icon';

export const InputComponent = styled.div`
    position: relative;
    margin-right: 15px;

    &:hover {
        .InputElement {
            background-color: #fff;
        }
    }
`

export const InputIconContainer = styled.div`
    position: absolute;
    left: 0;
    top: 0%;
    width: 32px;
    height: 32px;
    line-height: 32px;
    z-index: 1;
    display: flex;
    align-content: center;
    justify-content: center;
    cursor: pointer;

    .Icon {
        font-size: 18px;
        line-height: 1;
    }
`

export const InputIcon = styled(Icon)`
    font-size: 18px;
    line-height: 1;
`

export const InputCloseButton = styled.div`
    position: absolute;
    right: 5px;
    top: 0;
    height: 32px;
    line-height: 32px;
    cursor: pointer;
`

export const InputElement = styled.input`
    border: none;
    outline: none;
    box-shadow: none;
    width: 100%;
    height: 32px;
    line-height: 32px;
    padding: 0 25px 0 37px;
    font-size: 12px;
    background-color: transparent;
    transition: background-color .4s;
    box-sizing: border-box;

    &:focus {
        background-color: #fff;
    }

    &::-webkit-input-placeholder{
        font-family: inherit;
        font-weight: 300;
        color: #aaa;
    }

    &::-moz-placeholder{
        font-family: inherit;
        font-weight: 300;
        color: #aaa;
    }

    &:-ms-input-placeholder{
        font-family: inherit;
        font-weight: 300;
        color: #aaa;
    }
`