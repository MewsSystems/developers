import { library } from '@fortawesome/fontawesome-svg-core'
import { faSearch, faTimes } from '@fortawesome/free-solid-svg-icons'

export type IconName = 'search' | 'times'

export const addIconsToLibrary = () => {
  library.add(faSearch, faTimes)
}
