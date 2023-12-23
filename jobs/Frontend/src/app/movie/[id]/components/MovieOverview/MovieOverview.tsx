import { DetailOverview, StyledTitle } from './MovieOverview.styles'
import { Divider, Typography } from '@/components'

type Props = {
  overview: string
}

export const MovieOverview = ({ overview }: Props) => (
  <>
    <StyledTitle>
      <Typography variant="secondarySpan">Overview</Typography>
    </StyledTitle>
    <Divider />
    <DetailOverview>
      <Typography>{overview}</Typography>
    </DetailOverview>
  </>
)
