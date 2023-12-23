import { Typography } from '@/components'
import { Container } from './NoMatch.styles'

export const NoMatch = () => (
  <Container>
    <Typography variant="primarySpan">
      No movies match your selected filters.
    </Typography>
    <Typography variant="secondarySpan">Please adjust the filters.</Typography>
  </Container>
)
