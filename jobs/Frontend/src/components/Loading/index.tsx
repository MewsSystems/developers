import LoadingIcon from "@components/Icons/LoadingIcon"

export const Loading = () => {
  return (
    <div className="flex items-center p-2 md:p-4">
      <LoadingIcon />
      <span className="ml-2 text-gray-600 md:ml-4">Loading ...</span>
    </div>
  )
}

export default Loading
