import 'styled-components';

import { Theme } from './theme/theme.ts';
import { CSSProp } from 'styled-components';

declare module 'styled-components' {
  export interface DefaultTheme extends Theme {}
}

declare module 'react' {
  interface DOMAttributes<T> {
    css?: CSSProp;
  }
}
