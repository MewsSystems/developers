export default (data, filter) => {
  if (!filter) {
    return data
  }

  return Object.keys(data)
    .filter(key => data[key].trend === filter)
    .reduce((newObject, key) => {
      newObject[key] = data[key]

      return newObject
    }, {})
}
