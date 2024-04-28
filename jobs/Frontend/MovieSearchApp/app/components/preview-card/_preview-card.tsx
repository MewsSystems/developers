import { Link } from "@remix-run/react"
import styles from "./_preview-card.module.css"
import { Poster } from "~/components/poster"

type Props = {
  id: number
  title: string
  releaseDate: string
  overview: string
  posterImage: string | null
}

export const PreviewCard = ({
  id,
  title,
  releaseDate,
  overview,
  posterImage,
}: Props) => {
  const formattedReleaseDate = new Date(releaseDate).toLocaleDateString(
    "en-US",
    {
      day: "numeric",
      month: "numeric",
      year: "numeric",
    }
  )

  return (
    <Link to={`/movie-details/${id}`} className={styles.container}>
      <section className={styles.posterWrapper}>
        <Poster src={posterImage} alt={`Poster for ${title}`} />
      </section>
      <section className={styles.info}>
        <h2 className={styles.title}>{title}</h2>
        <p className={styles.release}>{formattedReleaseDate}</p>
        <p className={styles.overview}>{overview}</p>
      </section>
    </Link>
  )
}
