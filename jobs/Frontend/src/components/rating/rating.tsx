import clsx from 'clsx'
import css from './rating.module.css'

interface RatingProps {
  rating: number
  size?: 'default' | 'tiny'
}

const starEmptyIcon = '\u{2606}'
const starHalfIcon = '\u{2606}'
const starFullIcon = '\u{2605}'

const Rating = ({ rating, size = 'default' }: RatingProps) => {
  // Calculate the number of filled stars, half-filled stars, and empty stars
  const filledStars = Math.floor(rating / 2)
  const halfStar = rating % 2 !== 0
  const emptyStars = 5 - filledStars - (halfStar ? 1 : 0)

  return (
    <div className={clsx(css.wrapper, css[size])}>
      {[...Array(filledStars)].fill(starFullIcon)}
      {halfStar && starHalfIcon}
      {[...Array(emptyStars)].fill(starEmptyIcon)}
    </div>
  )
}

export default Rating
