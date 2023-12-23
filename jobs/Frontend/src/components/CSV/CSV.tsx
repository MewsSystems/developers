type Props = {
  index: number
  value: string
}

/**
 *
 * Adds comma between values
 */
export const CSV = ({ index, value }: Props) => (
  <>
    {index ? ', ' : ''}
    {value}
  </>
)
