import styles from "./_details.module.css"
import { Poster } from "~/components/poster"
import { ScoreChart } from "~/components/score-chart"

type Props = {
  title: string
  year: number
  releaseDate: string
  genres?: string
  runtime: string | null
  posterImage: string | null
  tagline: string | null
  overview: string | null
  homepage: string | null
  director: string
  cast: string
  score: number
}

export const Details = ({
  title,
  year,
  releaseDate,
  genres,
  runtime,
  posterImage,
  tagline,
  overview,
  homepage,
  director,
  cast,
  score,
}: Props) => {
  const hasTagline = tagline !== null

  return (
    <section className={styles.container}>
      <section className={styles.posterWrapper}>
        <Poster src={posterImage} alt={`Poster for ${title}`} />
      </section>
      <section className={styles.content}>
        <article className={styles.main}>
          <h2 className={styles.title}>
            {title} ({year})
          </h2>
          <p className={styles.facts}>
            {releaseDate}
            {genres !== undefined && ` • ${genres}`}
            {runtime !== null && ` • ${runtime}`}
          </p>

          {homepage !== null && (
            <p className={styles.homepage}>
              <a href={homepage}>Homepage</a>
            </p>
          )}

          <div className={styles.scoreWrapper}>
            <ScoreChart score={score} />
          </div>

          {hasTagline && (
            <blockquote className={styles.tagline}>{tagline}</blockquote>
          )}
          <p className={styles.overview}>{overview}</p>
        </article>
      </section>
      <article className={styles.director}>
        <h3 className={styles.sectionTitle}>Director:</h3>
        <p className={styles.sectionText}>{director}</p>
      </article>
      <article className={styles.cast}>
        <h3 className={styles.sectionTitle}>Cast:</h3>
        <p className={styles.sectionText}>{cast}</p>
      </article>
    </section>
  )
}
