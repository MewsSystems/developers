import type { ReactNode } from "react"
import styles from "./_list-item.module.css"

type Props = {
  children: ReactNode
}

export const ListItem = ({ children }: Props) => {
  return <li className={styles.listItem}>{children}</li>
}
