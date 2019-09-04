export const getStoragedData = (key) => {
  try {
    const storagedData = localStorage.getItem(key)
    return JSON.parse(storagedData)
  }
  catch (error) {
    return null
  }
}