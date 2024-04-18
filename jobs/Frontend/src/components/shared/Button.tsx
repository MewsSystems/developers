import styled from 'styled-components';
import { Color } from '../../enums/style/color.ts';
import { BorderRadius } from '../../enums/style/borderRadius.ts';
import { Spacer } from '../../enums/style/spacer.ts';

export const Button = styled.button`
    display: inline-block;
    min-width: 3rem;
    text-align: center;
    margin-right: ${Spacer.Sm};
    padding: ${Spacer.Sm} ${Spacer.Md};
    border: 2px solid ${Color.Accent};
    color: ${Color.Accent};
    border-radius: 0 ${BorderRadius.Md};
    text-decoration: none;
    transition: background-color .15s, color .15s;

    &.active {
        background-color: ${Color.SecondaryAccent};
    }
    
    &:hover, 
    &:focus {
        color: ${Color.Background};
        background-color: ${Color.Accent};
    }
`;