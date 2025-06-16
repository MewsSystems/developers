import styled from "styled-components";
import { fontSizes } from "../tokens/fontSizes";
import { colors } from "../tokens/colors";

export const FooterWrapper = styled.footer`
    background-color: ${colors.bgSecondary};
    text-align: center;
    padding: 20px;
    font-size: ${fontSizes.xxs}
    color: #aaa;
`