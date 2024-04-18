import styled from 'styled-components';
import { Spacer } from '../../../enums/style/spacer.ts';

export const PaginationRow = styled.div`
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
`;

export const PaginationSeparator = styled.span`
    display: inline-block;
    text-align: center;
    margin-right: ${Spacer.Sm};
    line-height: 2rem;
    min-width: 1.5rem;
`;