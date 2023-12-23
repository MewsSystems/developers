import { Typography } from '@/components'
import { DetailHeader } from './MovieHeader.styles'
import { ProgressChart } from '../ProgressChart'
import { getFormattedDate, getHoursAndMinutes } from '@/app/utils'

type Props = {
  title: string
  vote_average: number
  release_date: string
  runtime: number
}

export const MovieHeader = ({
  title,
  vote_average,
  release_date,
  runtime,
}: Props) => {
  const releaseDate = getFormattedDate(new Date(release_date))
  const { hours: runtimeHours, minutes: runtimeMinutes } =
    getHoursAndMinutes(runtime)

  return (
    <DetailHeader>
      <Typography variant="secondaryHeading">{title}</Typography>
      <ProgressChart percentage={Math.round(vote_average * 10)} />
      <span>
        <Typography color="secondary">{releaseDate}</Typography>
        <Typography color="secondary">&nbsp;&nbsp;|&nbsp;&nbsp;</Typography>
        <Typography color="secondary">
          {runtimeHours}h {runtimeMinutes}m
        </Typography>
      </span>
    </DetailHeader>
  )
}
