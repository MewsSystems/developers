import { IMAGE_BASE_URL } from '@/lib/utils'
import { Film } from 'lucide-react'

type Props = {
  posterPath: string | null | undefined
  alt: string
  className?: string
  isDetailView?: boolean
}

export const MoviePoster = ({
  posterPath,
  alt,
  className,
  isDetailView,
}: Props) => {
  if (!posterPath) {
    return (
      <div
        className={
          isDetailView
            ? 'w-64 h-96 bg-gray-300 rounded-lg flex items-center justify-center'
            : 'w-full h-full flex items-center justify-center bg-gray-100'
        }
      >
        <Film size={48} className="text-gray-400" />
      </div>
    )
  }

  return (
    <img
      src={`${IMAGE_BASE_URL}w500${posterPath}`}
      alt={alt}
      className={className}
    />
  )
}
