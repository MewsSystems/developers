import styled from 'styled-components';
import { Spacer } from '../../../enums/style/spacer';
import { Breakpoint } from '../../../enums/style/breakpoint';
import { Button } from '../Button';

export const PaginationRow = styled.div`
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
`;

export const PaginationButton = styled(Button)`
    margin-right: ${Spacer.Xs};

    @media (min-width: ${Breakpoint.Sm}) {
        margin-right: ${Spacer.Sm};
    }
`;

export const PaginationSeparator = styled.span`
    display: inline-block;
    text-align: center;
    margin-right: ${Spacer.Xs};
    line-height: 2rem;
    min-width: 1rem;

    @media (min-width: ${Breakpoint.Sm}) {
        margin-right: ${Spacer.Sm};
        min-width: 1.5rem;
    }
`;