import { Cast } from './Cast'
import { Crew } from './Crew'

export interface Credits {
  id: number
  cast: Cast[]
  crew: Crew[]
}
