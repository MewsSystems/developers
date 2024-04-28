import type { ReactNode } from "react"
import styles from "./_list.module.css"

type Props = {
  children: ReactNode
}

export const List = ({ children }: Props) => {
  return <ul className={styles.list}>{children}</ul>
}
