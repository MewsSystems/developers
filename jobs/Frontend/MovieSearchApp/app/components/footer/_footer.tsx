import styles from "./_footer.module.css"

export const Footer = () => {
  return (
    <footer className={styles.container}>
      <p className={styles.text}>
        This app is using data from{" "}
        <a href="https://www.themoviedb.org/" className={styles.link}>
          TMDB
        </a>
      </p>
    </footer>
  )
}
