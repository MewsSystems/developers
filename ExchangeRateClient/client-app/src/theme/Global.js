// @flow strict

import { createGlobalStyle } from "styled-components";
import styledNormalize from "styled-normalize";

const GlobalStyle = createGlobalStyle`
	${styledNormalize};
	body {
		font-family: ${({ theme }) => `"${theme.typography.fontFamily.primary}"`}, sans-serif;
		color:  ${({ theme }) => theme.typography.colors.default};
	}
`;

export default GlobalStyle;
