import { CalendarIcon, StarIcon, FireIcon } from "@heroicons/react/24/outline"
import { Modal } from "@components/Modal"
import { ModalHeader } from "@components/Modal/ModalHeader"
import { ModalBody } from "@components/Modal/ModalBody"
import { ModalFooter } from "@components/Modal/ModalFooter"
import { Button } from "@components/Button"
import { Movie } from "@types"
import { formatDate } from "@utils/dates"

interface ViewModalProps {
  movie: Movie | null
  isOpen?: boolean
  onDismiss?: () => void
}

export const ViewModal = ({ movie, isOpen, onDismiss }: ViewModalProps) => {
  if (!movie) {
    return <></>
  }

  return (
    <Modal isOpen={isOpen} onDismiss={onDismiss}>
      <ModalHeader>{movie.title}</ModalHeader>
      <ModalBody className="mb-6 mt-4">
        <div className="mb-4 flex flex-col items-start sm:flex-row">
          <img
            src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
            alt={movie.title}
            className="mb-4 mr-0 h-auto w-40 flex-shrink-0 object-cover sm:mb-0 sm:mr-4 sm:w-48 md:w-60"
          />
          <div>
            <div className="mb-4 flex flex-col items-start gap-x-0 gap-y-2 text-sm text-gray-600 md:flex-row md:items-center md:gap-x-4 md:gap-y-0">
              {movie.release_date ? (
                <p className="flex items-center">
                  <CalendarIcon className="mr-1 h-5 w-5" />
                  <span title="Release Date">{formatDate(movie.release_date)}</span>
                </p>
              ) : null}
              <p className="flex items-center">
                <StarIcon className="mr-1 h-5 w-5" />
                <span title="Vote Average (Vote Count)">
                  {movie.vote_average} stars ({movie.vote_count})
                </span>
              </p>
              <p className="flex items-center">
                <FireIcon className="mr-1 h-5 w-5" />
                <span title="Popularity">{movie.popularity}</span>
              </p>
            </div>
            <p className="text-sm" title="Overview">
              {movie.overview}
            </p>
          </div>
        </div>
      </ModalBody>
      <ModalFooter className="mt-4 flex justify-end">
        <Button
          className="block w-full border-[1px] border-gray-300 bg-white text-sm text-gray-900 hover:bg-gray-100 md:w-fit"
          onClick={onDismiss}
        >
          Close
        </Button>
      </ModalFooter>
    </Modal>
  )
}

export default ViewModal
