import styled from 'styled-components';
import { Spacer } from '../../../enums/style/spacer';
import { BorderRadius } from '../../../enums/style/borderRadius';
import { Gradient } from '../../../enums/style/gradient';

export const MovieCastRow = styled.section`
    max-width: 100%;
    overflow-y: auto;
    display: flex;
    flex-direction: row;
    flex-wrap: nowrap;
    margin-bottom: ${Spacer.Md};
    background: ${Gradient.AccentsLinearTitled};
    border-radius: ${BorderRadius.Md};
    padding: ${Spacer.Md};
    scroll-behavior: smooth;
    scroll-padding-left: ${Spacer.Md};
    scroll-snap-type: x mandatory;
`;