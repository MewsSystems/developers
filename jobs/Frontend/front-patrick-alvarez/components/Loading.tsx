export const Loading = () => {
  return (
    <div className="flex justify-center items-center min-h-screen" role="status" aria-live="polite">
      <div 
        className="animate-spin rounded-full h-16 w-16 border-b-4 border-white"
        aria-label="Loading"
      />
      <span className="sr-only">Loading...</span>
    </div>
  )
}
