import styled from 'styled-components';
import { Color } from '../../enums/style/color.ts';
import { FontSize } from '../../enums/style/fontSize.ts';
import { Spacer } from '../../enums/style/spacer.ts';
import { BorderRadius } from '../../enums/style/borderRadius.ts';

export const Label = styled.label`
    font-size: ${FontSize.Lg};
    padding-right: ${Spacer.Sm};
`;

export const TextInput = styled.input`
    font-size: ${FontSize.Lg};
    border: 2px solid ${Color.Accent};
    background-color: ${Color.Background};
    color: ${Color.Primary};
    border-radius: ${BorderRadius.Md};
    padding: ${Spacer.Sm} ${Spacer.Md};
`;