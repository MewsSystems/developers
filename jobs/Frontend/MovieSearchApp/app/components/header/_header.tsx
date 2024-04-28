import type { ReactNode } from "react"
import styles from "./_header.module.css"

export const Header = () => {
  return (
    <header className={styles.container}>
      <h1 className={styles.title}>🎞️ Movie Search App</h1>
    </header>
  )
}
