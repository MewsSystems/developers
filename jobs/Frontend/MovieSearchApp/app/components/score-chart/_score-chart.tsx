import styles from "./_score-chart.module.css"

const CANVAS_SIZE = 56
const CANVAS_CENTER_POINT = CANVAS_SIZE / 2
const BACKGROUND_CIRCLE_RADIUS = CANVAS_SIZE / 2
const PROGRESS_CIRCLE_RADIUS = 12
const CENTER_CIRCLE_RADIUS = 16
const PROGRESS_CIRCLE_STROKE_WIDTH = PROGRESS_CIRCLE_RADIUS * 2
const CIRCUMFERENCE = 2 * 3.142 * PROGRESS_CIRCLE_RADIUS

type Props = {
  score: number
}

export const ScoreChart = ({ score }: Props) => {
  const roundedScore = Math.round(score * 10) / 10
  const percent = score * 10

  return (
    <div className={styles.container}>
      <svg
        className={styles.chart}
        height="100%"
        viewBox={`0 0 ${CANVAS_SIZE} ${CANVAS_SIZE}`}
        width="100%"
      >
        <circle
          cx={CANVAS_CENTER_POINT}
          cy={CANVAS_CENTER_POINT}
          r={BACKGROUND_CIRCLE_RADIUS}
        />
        <circle
          className={styles.progressCircle}
          cx={CANVAS_CENTER_POINT}
          cy={CANVAS_CENTER_POINT}
          r={PROGRESS_CIRCLE_RADIUS}
          strokeDasharray={`${CIRCUMFERENCE * (percent / 100)} ${CIRCUMFERENCE}`}
          strokeWidth={PROGRESS_CIRCLE_STROKE_WIDTH}
        />
        <circle
          className={styles.centerCircle}
          cx={CANVAS_CENTER_POINT}
          cy={CANVAS_CENTER_POINT}
          r={CENTER_CIRCLE_RADIUS}
        />
      </svg>
      <div className={styles.content}>
        {roundedScore > 0 ? roundedScore : "NR"}
      </div>
    </div>
  )
}
