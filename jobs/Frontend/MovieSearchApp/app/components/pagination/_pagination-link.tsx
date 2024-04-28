import { Link, LinkProps } from "@remix-run/react"
import styles from "./_pagination-link.module.css"

type Props = {
  children: React.ReactNode
  disabled: boolean
  to: LinkProps["to"]
}

export const PaginationLink = ({ children, disabled, to }: Props) => {
  return disabled ? (
    <p className={styles.disabledLink}>{children}</p>
  ) : (
    <Link className={styles.link} to={to}>
      {children}
    </Link>
  )
}
