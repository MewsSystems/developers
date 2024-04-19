import styled from 'styled-components';
import { Spacer } from '../../../enums/style/spacer';
import { BsFillPersonFill } from 'react-icons/bs';

export const MovieCastCard = styled.div`
    min-width: 10rem;
    max-width: 10rem;
    min-height: 18rem;
    margin-right: ${Spacer.Md};
    border-radius: ${Spacer.Md} 0;
    overflow: hidden;
    background: rgba(255, 255, 255, 0.6);
    scroll-snap-align: start;

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