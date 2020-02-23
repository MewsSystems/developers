import { library } from '@fortawesome/fontawesome-svg-core'
import {
  faSearch,
  faTimes,
  faStar,
  faStarHalfAlt,
} from '@fortawesome/free-solid-svg-icons'
import { faStar as farStar } from '@fortawesome/free-regular-svg-icons'

export type IconName =
  | 'search'
  | 'times'
  | 'star'
  | ['far', 'star']
  | 'star-half-alt'

export const addIconsToLibrary = () => {
  library.add(faSearch, faTimes, faStar, farStar, faStarHalfAlt)
}
