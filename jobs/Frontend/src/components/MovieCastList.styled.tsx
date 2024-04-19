import styled from 'styled-components';
import { Spacer } from '../enums/style/spacer.ts';
import { BsFillPersonFill } from 'react-icons/bs';
import { BorderRadius } from '../enums/style/borderRadius.ts';
import { Gradient } from '../enums/style/gradient.ts';

export const MovieCastRow = styled.div`
    max-width: 100%;
    overflow-y: auto;
    display: flex;
    flex-direction: row;
    flex-wrap: nowrap;
    margin-bottom: ${Spacer.Md};
    background: ${Gradient.AccentsLinearTitled};
    border-radius: ${BorderRadius.Md};
    padding: ${Spacer.Md};
`;

export const MovieCastCard = styled.div`
    min-width: 10rem;
    max-width: 10rem;
    min-height: 18rem;
    margin-right: ${Spacer.Md};
    border-radius: ${Spacer.Md} 0;
    overflow: hidden;
    background: rgba(255, 255, 255, 0.6);
    
    &:last-child {
        margin-right: 0;
    }
`;

export const MovieCastCardBody = styled.div`
    padding: ${Spacer.Sm};
`;

export const CastMemberName = styled.h4`
    margin: 0 0 ${Spacer.Sm} 0;
`;

export const CastMemberPlaceholderIcon = styled(BsFillPersonFill)`
    font-size: 3rem;
`;