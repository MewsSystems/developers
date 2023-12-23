import { Typography, CSV } from '@/components'
import type { Credits, Genre } from '@/lib/types'
import { StyledTable, StyledTd } from './CreditsTable.styles'

type Props = {
  credits: Credits
  genres: Genre[]
}

export const CreditsTable = ({ credits, genres }: Props) => {
  const { cast, crew } = credits

  // cast and crew are already correctly sorted from BE
  const starring = cast.map((person) => person.name).slice(0, 4)

  // max 2 from each department
  const directing = crew
    .filter((person) => person.department === 'Directing')
    .map((person) => person.name)
    .slice(0, 2)
  const writing = crew
    .filter((person) => person.department === 'Writing')
    .map((person) => person.name)
    .slice(0, 2)

  // using set to remove duplicates
  const createdBy = [...new Set([...directing, ...writing])]

  return (
    <StyledTable>
      <tbody>
        <tr>
          <StyledTd>
            <Typography color="secondary">Starring</Typography>
          </StyledTd>
          <StyledTd>
            <Typography>
              {starring.map((name, index) => (
                <CSV value={name} key={index} index={index} />
              ))}
            </Typography>
          </StyledTd>
        </tr>
        <tr>
          <StyledTd>
            <Typography color="secondary">Created by</Typography>
          </StyledTd>
          <StyledTd>
            <Typography>
              {createdBy.map((name, index) => (
                <CSV value={name} key={index} index={index} />
              ))}
            </Typography>
          </StyledTd>
        </tr>
        <tr>
          <StyledTd>
            <Typography color="secondary">Genre</Typography>
          </StyledTd>
          <StyledTd>
            <Typography>
              {genres.map(({ name, id }, index) => (
                <CSV value={name} key={id} index={index} />
              ))}
            </Typography>
          </StyledTd>
        </tr>
      </tbody>
    </StyledTable>
  )
}
