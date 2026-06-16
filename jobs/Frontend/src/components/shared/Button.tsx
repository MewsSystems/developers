import styled from 'styled-components';
import { Color } from '../../enums/style/color';
import { BorderRadius } from '../../enums/style/borderRadius';
import { Spacer } from '../../enums/style/spacer';
import { FontSize } from '../../enums/style/fontSize';
import { Breakpoint } from '../../enums/style/breakpoint';

export const Button = styled.button`
    display: inline-block;
    min-width: 2.5rem;
    text-align: center;
    margin-right: ${Spacer.Sm};
    padding: ${Spacer.Sm};
    border: 2px solid ${Color.Accent};
    border-radius: 0 ${BorderRadius.Md};
    color: ${Color.Accent};
    background-color: ${Color.Background};
    text-decoration: none;
    font-size: ${FontSize.Md};
    cursor: pointer;
    transition: background-color .15s, color .15s;

    @media (min-width: ${Breakpoint.Sm}) {
        padding: ${Spacer.Sm} ${Spacer.Md};
        min-width: 3rem;
    }

    &.active {
        background-color: ${Color.SecondaryAccent};
    }

    &:last-child {
        margin-right: 0;
    }

    &:hover,
    &:focus {
        color: ${Color.Background};
        background-color: ${Color.Accent};
    }
`;