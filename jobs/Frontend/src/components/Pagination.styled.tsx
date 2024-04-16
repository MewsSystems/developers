import styled, { css } from 'styled-components';
import { Link } from 'react-router-dom';
import { Color } from '../enums/style/color.ts';
import { Spacer } from '../enums/style/spacer.ts';
import { BorderRadius } from '../enums/style/borderRadius.ts';

export const PaginationRow = styled.div`
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
`;

const paginationItemStyles = css`
    display: inline-block;
    min-width: 3rem;
    text-align: center;
    margin-right: ${Spacer.Sm};
    line-height: 1.75rem;
`;

export const PaginationSeparator = styled.span`
    ${paginationItemStyles};
    min-width: 1.5rem;
`;

export const ButtonLink = styled(Link)`
    ${paginationItemStyles};
    border: 2px solid ${Color.Accent};
    color: ${Color.Accent};
    border-radius: ${BorderRadius.Md};
    text-decoration: none;
    transition: background-color .15s, color .15s;

    &.active {
        color: ${Color.Primary};
        background-color: ${Color.SecondaryAccent};
    }
    
    &:hover, 
    &:focus {
        color: ${Color.Background};
        background-color: ${Color.Accent};
    }
`;