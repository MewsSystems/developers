import styled, { keyframes } from 'styled-components';
import { Color } from '../../enums/style/color.ts';
import { Spacer } from '../../enums/style/spacer.ts';

export const LoaderWrapper = styled.div`
    vertical-align: middle;
`;

const spinning = keyframes`
    to { 
        transform: rotate(360deg); 
    }
`;

export const LoadingIndicator = styled.div`
    display: inline-block;
    width: 2rem;
    height: 2rem;
    margin-left: ${Spacer.Md};
    border: .25rem solid transparent;
    border-radius: 50%;
    border-color: ${Color.Accent} transparent transparent ${Color.Accent};
    animation: ${spinning} 1.25s linear infinite; 
`;