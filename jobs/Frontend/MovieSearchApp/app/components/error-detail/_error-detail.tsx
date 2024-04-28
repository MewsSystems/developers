import styles from "./_error-detail.module.css"

type Props = {
  message: string | undefined
  statusCode: number | undefined
  statusText: string | undefined
}

export const ErrorDetail = ({
  message = "An error occurred",
  statusCode,
  statusText,
}: Props) => {
  return (
    <section className={styles.container}>
      {statusCode !== undefined && (
        <h2 className={styles.title}>{statusCode}</h2>
      )}
      {statusText !== undefined && (
        <h3 className={styles.subtitle}>{statusText}</h3>
      )}
      <p className={styles.message}>{message}</p>
    </section>
  )
}
