export const getFormattedDate = (date: Date) => {
  const formattedDate = date.toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  })

  return formattedDate
}
