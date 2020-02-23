import { useSelector } from 'react-redux'
import { State } from 'state/rootReducer'
import { ConfigurationLogoSizesEnum } from 'model/api/ConfigurationLogoSizesEnum'
import { ConfigurationPosterSizesEnum } from 'model/api/ConfigurationPosterSizesEnum'
import { ConfigurationBackdropSizesEnum } from 'model/api/ConfigurationBackdropSizesEnum'
import { ConfigurationProfileSizesEnum } from 'model/api/ConfigurationProfileSizes'
import { ConfigurationStillSizesEnum } from 'model/api/ConfigurationStillSizesEnum'

export type SizeEnum =
  | ConfigurationBackdropSizesEnum
  | ConfigurationLogoSizesEnum
  | ConfigurationPosterSizesEnum
  | ConfigurationProfileSizesEnum
  | ConfigurationStillSizesEnum

export const useGetImagePath = (size: SizeEnum) => {
  const {
    images: { base_url },
  } = useSelector((state: State) => state.configuration)

  return (url: string) => `${base_url}${size}${url}`
}
