import { ConfigurationDetails } from "~/utils/get-configuration-details.server"

type Args = {
  imagePath: string | null
  size: 0 | 1 | 2 | 3 | 4 | 5 | 6
  configurationDetails: ConfigurationDetails
}

export const getPosterImageSource = ({
  size,
  imagePath,
  configurationDetails,
}: Args) => {
  if (imagePath === null) {
    return null
  }

  const baseUrl = `${configurationDetails.images.secure_base_url}`
  const posterSize = configurationDetails.images.poster_sizes[size]
  const formattedPath = imagePath.replace(/\//g, "")

  return `${baseUrl}${posterSize}/${formattedPath}`
}
