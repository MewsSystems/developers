export const getStoragedConfig = () => {
  try {
    const storagedConfig = localStorage.getItem('currencyPairs')
    return JSON.parse(storagedConfig)
  }
  catch (error) {
    return null
  }
}