import { ReactNode } from 'react';

import css from './page.module.css';

interface Props {
  children: ReactNode;
}

export const PageContainer = ({ children }: Props) => (
  <div className={css.container}>{children}</div>
);
