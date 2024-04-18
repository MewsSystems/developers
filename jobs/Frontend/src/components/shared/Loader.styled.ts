import styled, { keyframes } from 'styled-components';
import { Color } from '../../enums/style/color.ts';

export const LoaderWrapper = styled.div`
    text-align: center;
`;

const spinning = keyframes`
    to { 
        transform: rotate(360deg); 
    }
`;

export const LoadingIndicator = styled.div`
    display: inline-block;
    width: 6rem;
    height: 6rem;
    border: .5rem solid rgba(15,15,15,.3);
    border-radius: 50%;
    border-top-color: ${Color.Accent};
    animation: ${spinning} 1.5s linear infinite; 
`;