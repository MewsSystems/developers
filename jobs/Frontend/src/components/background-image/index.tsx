import React from 'react';
import styled from 'styled-components';
import { Props } from './model';


export const BackgroundImage = (props: Props) => {
    return (
        <Wrapper url={props.url}>
            {props.children}
        </Wrapper>
    );
};

const Wrapper = styled.div<{ url: string }>`
    background-image: url(${props => props.url});
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;
    height: 100vh;
    width: 100%;
    overflow-x: hidden;

    &::before {
        content: '';
        background-color: rgba(255, 255, 255, 0.5);
        position: absolute;
        height: 100%;
        width: 100%;
        top: 0;
        left: 0;
    }
`;
