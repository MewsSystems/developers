import { useAppDispatch, useAppSelector } from "../app/hooks"
import { addResults } from "../app/slices/movieList/thunks"

const useAddResults = () => {
  const dispatch = useAppDispatch()
  const { morePages } = useAppSelector(state => state.movieList)

  const disableButton = !morePages

  const handleAddResults = () => {
    dispatch(addResults())
  }

  return [disableButton, handleAddResults] as const
}

export default useAddResults
