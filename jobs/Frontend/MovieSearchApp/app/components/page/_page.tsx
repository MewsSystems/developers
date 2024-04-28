import type { ReactNode } from "react"
import styles from "./_page.module.css"

type Props = {
  children: ReactNode
}

export const Page = ({ children }: Props) => {
  return <main className={styles.container}>{children}</main>
}
