import { ReactNode } from 'react';

import css from './page.module.css';

interface Props {
  children: ReactNode;
}

export const Page = ({ children }: Props) => (
  <div className={css.container}>{children}</div>
);
