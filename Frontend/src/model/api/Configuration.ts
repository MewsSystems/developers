import { ConfigurationBackdropSizesEnum } from './ConfigurationBackdropSizesEnum'
import { ConfigurationLogoSizesEnum } from './ConfigurationLogoSizesEnum'
import { ConfigurationPosterSizesEnum } from './ConfigurationPosterSizesEnum'
import { ConfigurationProfileSizesEnum } from './ConfigurationProfileSizes'
import { ConfigurationStillSizesEnum } from './ConfigurationStillSizesEnum'

export interface Configuration {
  images: {
    base_url: string
    secure_base_url: string
    backdrop_sizes: ConfigurationBackdropSizesEnum[]
    logo_sizes: ConfigurationLogoSizesEnum[]
    poster_sizes: ConfigurationPosterSizesEnum[]
    profile_sizes: ConfigurationProfileSizesEnum[]
    still_sizes: ConfigurationStillSizesEnum[]
  }
  change_keys: string[]
}
